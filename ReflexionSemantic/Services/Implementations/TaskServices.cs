using ReflexionSemantic.Dtos.Request;
using ReflexionSemantic.Services.Interfaces;
using Flurl.Http;
using ReflexionSemantic.Dtos.Response;
using ReflexionSemantic.Models;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using ReflexionSemantic.Repositories;
using MongoDB.Driver;

namespace ReflexionSemantic.Services.Implementations
{
    public class TaskServices : ITaskServices
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoRepository<Indexes> _indexRepo;
        private readonly IMongoRepository<Videos> _videoRepo;

        public TaskServices(IConfiguration configuration, IMongoRepository<Indexes> indexRepo, IMongoRepository<Videos> videoRepo)
        {
            _configuration = configuration;
            _indexRepo = indexRepo;
            _videoRepo = videoRepo;
        }

        public List<Indexes> GetIndexesAsync()
        {
            return _indexRepo.AsQueryable().ToList();
        }

        public List<Videos> GetVideosTasksAsync()
        {
            return _videoRepo.AsQueryable().ToList();
        }

        public List<AllVideosOfIndexDto> GetVideosbyIndexNameAsync(string IndexName)
        {
            List<AllVideosOfIndexDto> allVideos = new List<AllVideosOfIndexDto>();
            var index = _indexRepo.AsQueryable().FirstOrDefault(x => x.IndexName == IndexName);
            var data = _videoRepo.AsQueryable().Where(x => x.TwelvelabIndexId == index.TwelvelabIndexId).ToList();
            var ApiURL = _configuration.GetValue<string>("API_URL");
            var ApiKey = _configuration.GetValue<string>("API_KEY");

            foreach (var item in data)
            {  
                allVideos.Add(new AllVideosOfIndexDto
                {
                    IndexId = item.TwelvelabIndexId,
                    IndexName = IndexName,
                    ThumbnailUrl = string.Empty,
                    VideoId = item.TwelvelabVideoId,
                    VideoUrl = item.VideoBlobUrl
                });
            }
            return allVideos;
        }

        public async Task<SearchResponseDto> Search(SearchRequestDto searchRequestDto)
        {
            SearchResponseDto searchResponseDto = new();
            var ApiURL = _configuration.GetValue<string>("API_URL");
            var ApiKey = _configuration.GetValue<string>("API_KEY");
            var indexId = _indexRepo.AsQueryable().FirstOrDefault();
            var searchURL = ApiURL + "/search";
            if (searchRequestDto.search_options.Contains("conversation"))
            {
                searchRequestDto.conversation_option = "semantic";
            }
            searchRequestDto.index_id = indexId.TwelvelabIndexId;
            var query = await searchURL.WithHeader("x-api-key", ApiKey).AllowAnyHttpStatus().PostJsonAsync(searchRequestDto);

            if(query.StatusCode <300)
            {
                searchResponseDto = await query.GetJsonAsync<SearchResponseDto>();
                var VideoList = _videoRepo.AsQueryable().Where(x => x.TwelvelabIndexId == searchRequestDto.index_id).ToList();

                foreach (var item in searchResponseDto?.data)
                {
                    var video = VideoList.Where(x => x.TwelvelabVideoId == item.video_id).FirstOrDefault();
                    item.video_blob_url = video?.VideoBlobUrl;
                }
            }
            return searchResponseDto;
        }

        public async Task<CreateIndexResponseDto> CreateIndex(CreateIndexRequestDto requestDto)
        {
            var ApiURL = _configuration.GetValue<string>("API_URL");
            var ApiKey = _configuration.GetValue<string>("API_KEY");

            var IndexesURL = ApiURL + "/indexes";
            var INDEX_NAME = requestDto.IndexName;
            var indexoptions = createIndexoptons(requestDto);
            CreateIndexTwelveLabsRequestDto body = new()
            {
                engine_id = requestDto.engine_id,
                index_name = INDEX_NAME,
                index_options = indexoptions
            };

            var query = await IndexesURL.WithHeader("x-api-key", ApiKey).AllowAnyHttpStatus().PostJsonAsync(body);

            if (query.StatusCode < 300)
            {
                var response = await query.GetJsonAsync<CreateIndexResponseDto>();
                _indexRepo.InsertOne(new Indexes()
                {
                    CreatedTs = DateTime.UtcNow,
                    IndexName = INDEX_NAME,
                    EngineId = requestDto.engine_id,
                    Indexoptions = JsonConvert.SerializeObject(indexoptions),
                    TwelvelabIndexId = response._id,
                    IsActive = true
                });
                return response;
            }

            return new CreateIndexResponseDto();
        }

        public async Task<Videos> UploadVideo(IFormFile file)
        {
            var ApiURL = _configuration.GetValue<string>("API_URL");
            var ApiKey = _configuration.GetValue<string>("API_KEY");
            var AzureWebJobsStorage = _configuration.GetValue<string>("AzureWebJobsStorage");
            var ContainerName = _configuration.GetValue<string>("ContainerName");

            var TASKS_URL = ApiURL + "/tasks";
            Stream myBlob = new MemoryStream();
            myBlob = file.OpenReadStream();
            var filename = DateTime.Now.Ticks.GetHashCode().ToString("x").ToUpper() + Path.GetExtension(file.FileName);

            var blobClient = new BlobContainerClient(AzureWebJobsStorage, ContainerName);
            var blob = blobClient.GetBlobClient(filename);
            await blob.UploadAsync(myBlob);
            
            var indexId = _indexRepo.AsQueryable().FirstOrDefault();

            var query = await TASKS_URL
                                             .WithHeader("x-api-key", ApiKey).AllowAnyHttpStatus()
                                             .PostMultipartAsync(mp =>
                                                mp.AddString("video_url", blob.Uri.AbsoluteUri)
                                                  .AddString("language", "en")
                                                  .AddString("index_id", indexId?.TwelvelabIndexId));

            if (query.StatusCode < 300)
            {
                var response = await query.GetJsonAsync<UploadvideoResponseDto>();
                
                var videos = new Videos()
                {
                    CreatedTs = DateTime.UtcNow,
                    TwelvelabIndexId = indexId?.TwelvelabIndexId,
                    FileName = file.FileName,
                    EstimatedTime = response.estimated_time,
                    Metadata = JsonConvert.SerializeObject(response.metadata),
                    TwelvelabTaskId = response._id,
                    is_ready = false,
                    VideoBlobUrl = blob.Uri.AbsoluteUri,
                    IsActive = true
                };  
                _videoRepo.InsertOne(videos);                
                return videos;

            }
            return new Videos();
        }

        public async Task<bool> CheckVideoStatus()
        {
            var pendingVideos = _videoRepo.AsQueryable().Where(x => x.is_ready == false).ToList();
            var ApiURL = _configuration.GetValue<string>("API_URL");
            var ApiKey = _configuration.GetValue<string>("API_KEY");
            bool changes = false;
            foreach (var item in pendingVideos)
            {
                var TASK_STATUS_URL = ApiURL + "/tasks/" + item.TwelvelabTaskId;

                var query = await TASK_STATUS_URL
                                                 .WithHeader("x-api-key", ApiKey).AllowAnyHttpStatus()
                                                 .GetAsync();
                if (query.StatusCode < 300)
                {
                    var response = await query.GetJsonAsync<TaskStatusResponseDto>();
                    item.status = response.status;
                    if (response?.hls is not null && response?.hls?.status == "COMPLETE")
                    {
                        item.ThumbnailUrl = response.hls?.thumbnail_urls.FirstOrDefault() ?? string.Empty;
                        item.TwelvelabVideoUrl = response.hls?.video_url;
                    }
                    if (response.status == "ready")
                    {
                        item.RawData = JsonConvert.SerializeObject(response);
                        item.is_ready = true;
                        item.TwelvelabVideoId = response.video_id;                        
                        changes = true;                        
                    }
                    await _videoRepo.ReplaceOneAsync(item);
                }
            }
            return changes;
        }

        #region privateMethods
        private List<string> createIndexoptons(CreateIndexRequestDto requestDto)
        {
            var indexOptionList = new List<string>();

            if (requestDto.visual)
            {
                indexOptionList.Add("visual");
            }
            if (requestDto.conversation)
            {
                indexOptionList.Add("conversation");
            }
            if (requestDto.text_in_video)
            {
                indexOptionList.Add("text_in_video");
            }
            if (requestDto.logo)
            {
                indexOptionList.Add("logo");
            }
            return indexOptionList;
        }
        #endregion
    }
}

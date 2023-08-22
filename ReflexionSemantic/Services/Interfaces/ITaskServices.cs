using ReflexionSemantic.Dtos.Request;
using ReflexionSemantic.Dtos.Response;
using ReflexionSemantic.Models;

namespace ReflexionSemantic.Services.Interfaces
{
    public interface ITaskServices
    {
        Task<CreateIndexResponseDto> CreateIndex(CreateIndexRequestDto requestDto);
        Task<Videos> UploadVideo(IFormFile file);
        List<Indexes> GetIndexesAsync();
        List<Videos> GetVideosTasksAsync();
        Task<SearchResponseDto> Search(SearchRequestDto searchRequestDto);
        List<AllVideosOfIndexDto> GetVideosbyIndexNameAsync(string IndexName);
        Task<bool> CheckVideoStatus();
    }
}

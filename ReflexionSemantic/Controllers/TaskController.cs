using Microsoft.AspNetCore.Mvc;
using ReflexionSemantic.Dtos.Request;
using ReflexionSemantic.Services.Interfaces;

namespace ReflexionSemantic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskServices _services;

        public TaskController(ITaskServices services)
        {
            _services = services;
        }

        [HttpPost("Index")]
        public async Task<IActionResult> CreateIndex([FromBody] CreateIndexRequestDto requestDto)
        {
            return Ok(await _services.CreateIndex(requestDto));
        }
        [HttpGet("Index")]
        public IActionResult GetIndex()
        {
            return Ok(_services.GetIndexesAsync());
        }
        [HttpGet("VideoTaks")]
        public IActionResult GetVideoTaks()
        {
            return Ok(_services.GetVideosTasksAsync());
        }

        [HttpPost("upload")]
        [RequestSizeLimit(Int64.MaxValue)]
        public async Task<IActionResult> UplaodVideo(IFormFile file)
        {
            return Ok(await _services.UploadVideo(file));
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchRequestDto searchRequestDto)
        {
            return Ok(await _services.Search(searchRequestDto));
        }

        [HttpGet("videos/getall")]
        public IActionResult videosGetAll()
        {
            return Ok(_services.GetVideosTasksAsync());
        }

        [HttpGet("videos/checkstatus")]
        public async Task<IActionResult> checkvideostatus()
        {
            return Ok(await _services.CheckVideoStatus());
        }
    }
}

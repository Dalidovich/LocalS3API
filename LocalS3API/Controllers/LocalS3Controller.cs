using LocalS3API.Service;
using Microsoft.AspNetCore.Mvc;

namespace LocalS3API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LocalS3Controller : ControllerBase
    {
        private readonly S3FileService _fileService;

        public LocalS3Controller(S3FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload/file")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string bucket)
        {
            var response = await _fileService.UploadFiles(new() { file }, bucket);

            return Ok(response);
        }

        [HttpPost("upload/files")]
        public async Task<IActionResult> Upload([FromForm] List<IFormFile> files, [FromForm] string bucket)
        {
            var response = await _fileService.UploadFiles(files, bucket);

            return Ok(response);
        }

        [HttpGet("load/file")]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Client, Duration = 60 * 2)]
        public IActionResult LoadFile([FromQuery] string id, [FromQuery] string bucket)
        {
            var response = _fileService.GetFile(id, bucket);

            if (response.FileStream != null)
            {
                return File(response.FileStream, response.ContentType);
            }

            return NotFound();
        }

        [HttpGet("load/files")]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Client, Duration = 60 * 2)]
        public IActionResult LoadFiles([FromQuery] string bucket)
        {
            var response = _fileService.GetFiles(bucket);

            return Ok(response);
        }

        [HttpDelete("bucket/single")]
        public IActionResult DeleteBucket([FromQuery] string bucket)
        {
            _fileService.DeleteBucket(bucket);

            return NoContent();
        }

        [HttpDelete("bucket/any")]
        public IActionResult DeleteBuckets([FromQuery] string[] buckets)
        {
            _fileService.DeleteBuckets(buckets);

            return NoContent();
        }

        [HttpDelete("file/single")]
        public IActionResult DeleteFile([FromQuery] string id, [FromQuery] string bucket)
        {
            _fileService.DeleteFile(id, bucket);

            return NoContent();
        }

        [HttpPut("file/replace")]
        public async Task<IActionResult> ReplaseFile([FromQuery] string id, [FromQuery] string bucket, [FromForm] IFormFile file)
        {
            var response = await _fileService.ReplaseFile(id, bucket, file);

            return response? Ok(): NotFound();
        }
    }
}

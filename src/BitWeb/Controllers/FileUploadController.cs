using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FileUploadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> _logger;

        public FileUploadController(ILogger<FileUploadController> logger)
        {
            _logger = logger;
        }

        [HttpGet("test")]
        public IActionResult Get()
        {
            return Ok("Done");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            _logger.LogInformation("File upload initiated.");

            // Process the file line by line asynchronously
            await foreach (var line in ProcessFileAsync(file))
            {
                _logger.LogInformation($"Processing line: {line}");
            }

            _logger.LogInformation("File upload and processing completed.");
            return Ok("File uploaded and processed successfully.");
        }

        private async IAsyncEnumerable<string> ProcessFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                // Yield the line so it can be processed
                yield return line;
            }
        }
    }
}

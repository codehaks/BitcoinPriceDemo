﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BitWeb.Domain;

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

            var backgroundTask = Task.Run(() => SimulateBackgroundWork());


            // Process the file line by line asynchronously
            await foreach (var line in ProcessFileAsync(file))
            {
                var data = CsvDeserializer.DeserializeLine(line);
               // await Task.Delay(500);
                _logger.LogInformation($"Current Price line: {data.Close}");
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

        [HttpPost("upload2")]
        public async Task<IActionResult> UploadFile2(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            _logger.LogInformation("File upload initiated.");
            var backgroundTask = Task.Run(() => SimulateBackgroundWork());

            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var data = CsvDeserializer.DeserializeLine(line);
                  //  await Task.Delay(500); // Simulate some processing delay
                    _logger.LogInformation($"Current Price line: {data.Close}");
                }
            }

            _logger.LogInformation("File upload and processing completed.");
            return Ok("File uploaded and processed successfully.");
        }

        private async Task SimulateBackgroundWork()
        {
            for (int i = 0; i < 10; i++)
            {
                _logger.LogInformation($"Background task running at {DateTime.Now}");
                await Task.Delay(1000); // Simulate a background task that runs every second
            }
        }
    }
}

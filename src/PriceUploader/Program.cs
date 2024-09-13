// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");

// File name and URL
string fileName = "BTC-2021min.csv";
string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
string url = "https://localhost:7073/api/FileUpload/upload";

if (!File.Exists(filePath))
{
    Console.WriteLine($"File {fileName} not found.");
}

Console.WriteLine($"Uploading file: {fileName}...");

using var httpClient = new HttpClient();
// Increase the timeout to avoid issues with large files
httpClient.Timeout = TimeSpan.FromMinutes(10); // Adjust as needed

using var content = new MultipartFormDataContent();
// Read file content
using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
var fileContent = new StreamContent(fileStream);
fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

// Add file to form data
content.Add(fileContent, "file", fileName);


// Send POST request
HttpResponseMessage response = await httpClient.PostAsync(url, content);

// Check response
if (response.IsSuccessStatusCode)
{
    Console.WriteLine("File uploaded successfully.");
}
else
{
    Console.WriteLine($"File upload failed: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
}

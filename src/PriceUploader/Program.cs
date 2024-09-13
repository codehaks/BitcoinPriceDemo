// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// File name and URL
string fileName = "BTC-2021min.csv";
string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
string url = "https://localhost:5001/upload";

if (File.Exists(filePath))
{
    Console.WriteLine($"Uploading file: {fileName}...");

    using var httpClient = new HttpClient();

    // Create a Multipart form data content
    using var content = new MultipartFormDataContent();

    // Read file content
    var fileContent = new StreamContent(File.OpenRead(filePath));
    fileContent.Headers.Add("Content-Type", "text/csv");

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
        Console.WriteLine($"File upload failed: {response.StatusCode}");
    }
}
else
{
    Console.WriteLine($"File {fileName} not found.");
}
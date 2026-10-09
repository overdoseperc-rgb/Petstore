using System.Text;
using System.Text.Json;

public class BaseRequest : IDisposable
{
    private readonly HttpClient client = new HttpClient();

    public BaseRequest(string baseUrl)
    {
        client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
        client.Timeout = TimeSpan.FromSeconds(20);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("api_key", "special-key");
    }

    public async Task<JsonElement> Send(string method, string path,
        object data = null, int expectedStatus = 200)
    {
        using var request = new HttpRequestMessage(new HttpMethod(method), path);
        if (data != null)
        {
            string json = JsonSerializer.Serialize(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await client.SendAsync(request);
        string text = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{method} {path}: {(int)response.StatusCode}");

        if ((int)response.StatusCode != expectedStatus)
            throw new HttpRequestException($"Ожидался статус {expectedStatus}, получен {(int)response.StatusCode}: {text}", null, response.StatusCode);

        using var document = JsonDocument.Parse(text);
        return document.RootElement.Clone();
    }

    public void Dispose()
    {
        client.Dispose();
    }
}

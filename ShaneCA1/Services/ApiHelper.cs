using System.Net;
using System.Text.Json;

namespace ShaneCA1;

public class ApiException : Exception
{
    public ApiException(string message) : base(message)
    {
    }
}

public static class ApiHelper
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    public static async Task<T> GetAsync<T>(HttpClient http, string url, string serviceName)
    {
        try
        {
            HttpResponseMessage response = await http.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ApiException($"{serviceName} rejected our API key.");
            }
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new ApiException($"{serviceName} is busy. Please try again in a moment.");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException($"{serviceName} returned an error ({(int)response.StatusCode}).");
            }

            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] {serviceName} response: {json.Substring(0, Math.Min(500, json.Length))}...");
            
            T? result = JsonSerializer.Deserialize<T>(json, JsonOptions);

            if (result == null)
            {
                throw new ApiException($"{serviceName} sent back an empty response.");
            }

            return result;
        }
        catch (HttpRequestException)
        {
            throw new ApiException($"Could not connect to {serviceName}. Check your internet connection.");
        }
        catch (TaskCanceledException)
        {
            throw new ApiException($"{serviceName} took too long to respond.");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"[DEBUG] JSON Exception: {ex.Message}");
            throw new ApiException($"{serviceName} sent back data we could not read.");
        }
    }
}
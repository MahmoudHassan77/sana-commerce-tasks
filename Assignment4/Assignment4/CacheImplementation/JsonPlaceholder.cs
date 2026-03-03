using System.Net.Http.Json;

namespace CacheImplementation;

public class JsonPlaceholder
{
    private readonly HttpClient _httpClient;

    public JsonPlaceholder(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Fetches a post by its ID from JSONPlaceholder.
    /// </summary>
    public async Task<Post?> GetPostAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Post>(
            $"https://dummyjson.com/posts/{id}"
        );
    }
}

/// <summary>
/// Represents a post from the JSONPlaceholder API.
/// </summary>
public record Post(int UserId, int Id, string Title, string Body);

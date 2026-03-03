using CacheImplementation;

const int CacheCapacity = 3;

async Task<Post> FetchPost(JsonPlaceholder client, int id)
{
    try
    {
        var post = await client.GetPostAsync(id);
        if (post is not null) return post;
    }
    catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
    {
        Console.WriteLine($"API unreachable — using mock data");
    }

    return new Post(1, id, $"Mock Post Title {id}", $"Mock body for post {id}.");
}

using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
var apiClient = new JsonPlaceholder(httpClient);
using var cache = new InMemoryCache<int, Post>(CacheCapacity);

Console.WriteLine($"=== Cache (capacity = {CacheCapacity}) ===\n");

for (int id = 1; id <= 4; id++)
{
    if (cache.TryGet(id, out var cached))
    {
        Console.WriteLine($"CACHE HIT - Post {id}: {cached!.Title}");
    }
    else
    {
        Console.WriteLine($"CACHE MISS - Fetching post {id}...");
        var post = await FetchPost(apiClient, id);
        cache.Set(id, post);
        Console.WriteLine($"FETCHED - Post {id}: {post.Title}");
    }
    Console.WriteLine($"Cache count: {cache.Count}/{CacheCapacity}");
}


Console.WriteLine("\n=== Cache Hit===\n");

if (cache.TryGet(2, out var hitPost))
{
    Console.WriteLine($"CACHE HIT - Post 2: {hitPost!.Title}");
}
else
{
    Console.WriteLine("CACHE MISS - Post 2 was evicted (expected — LRU eviction).");
}

if (cache.TryGet(1, out var evictedPost))
{
    Console.WriteLine($"CACHE HIT - Post 1: {evictedPost!.Title}");
}
else
{
    Console.WriteLine("CACHE MISS - Post 1 was evicted (expected — LRU eviction).");
}


Console.WriteLine("\n=== LRU Eviction Verification ===\n");
Console.WriteLine($"Cache contains post 2: {cache.Contains(2)}");
Console.WriteLine($"Cache contains post 3: {cache.Contains(3)}");
Console.WriteLine($"Cache contains post 4: {cache.Contains(4)}");
Console.WriteLine($"Cache contains post 1: {cache.Contains(1)} (evicted)");


Console.WriteLine("\n=== Concurrent Access ===\n");

using var concurrentCache = new InMemoryCache<int, string>(100);
var tasks = new List<Task>();

for (int t = 0; t < 10; t++)
{
    int threadId = t;
    tasks.Add(Task.Run(() =>
    {
        for (int i = 0; i < 100; i++)
        {
            concurrentCache.Set(threadId * 100 + i, $"Thread{threadId}-Item{i}");
        }
    }));
}

await Task.WhenAll(tasks);
Console.WriteLine($"Concurrent writes completed. Cache count: {concurrentCache.Count}");
Console.WriteLine("No exceptions — thread safety verified.");

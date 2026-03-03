using CacheImplementation;

namespace CacheImplementationTests;

public class CacheConcurrencyTests
{
    [Fact]
    public async Task ConcurrentSets_NoExceptions_CountDoesNotExceedCapacity()
    {
        const int capacity = 50;
        const int threadCount = 10;
        const int opsPerThread = 100;

        using var cache = new InMemoryCache<int, string>(capacity);
        var tasks = new Task[threadCount];

        for (int t = 0; t < threadCount; t++)
        {
            int threadId = t;
            tasks[t] = Task.Run(() =>
            {
                for (int i = 0; i < opsPerThread; i++)
                {
                    cache.Set(threadId * opsPerThread + i, $"T{threadId}-{i}");
                }
            });
        }

        await Task.WhenAll(tasks);

        Assert.True(cache.Count <= capacity,
            $"Cache count {cache.Count} exceeds capacity {capacity}");
        Assert.True(cache.Count > 0, "Cache should have entries");
    }

    [Fact]
    public async Task ConcurrentReadsAndWrites_NoExceptions()
    {
        const int capacity = 100;
        using var cache = new InMemoryCache<int, int>(capacity);

        // Pre-populate
        for (int i = 0; i < capacity; i++)
            cache.Set(i, i);

        var tasks = new List<Task>();

        // Writers
        for (int t = 0; t < 5; t++)
        {
            int offset = t * 200;
            tasks.Add(Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                    cache.Set(offset + i, i);
            }));
        }

        // Readers
        for (int t = 0; t < 5; t++)
        {
            tasks.Add(Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    cache.TryGet(i, out _);
                    _ = cache.Contains(i);
                    _ = cache.Count;
                }
            }));
        }

        await Task.WhenAll(tasks);

        Assert.True(cache.Count <= capacity);
    }
}

using CacheImplementation;

namespace CacheImplementationTests;

public class InMemoryCacheTests
{
    [Fact]
    public void Set_ThenTryGet_ReturnsStoredValue()
    {
        using var cache = new InMemoryCache<string, int>(10);
        cache.Set("key", 42);

        var found = cache.TryGet("key", out var value);

        Assert.True(found);
        Assert.Equal(42, value);
    }

    [Fact]
    public void TryGet_MissingKey_ReturnsFalse()
    {
        using var cache = new InMemoryCache<string, int>(10);

        var found = cache.TryGet("missing", out var value);

        Assert.False(found);
        Assert.Equal(default, value);
    }

    [Fact]
    public void Contains_ExistingKey_ReturnsTrue()
    {
        using var cache = new InMemoryCache<string, string>(10);
        cache.Set("a", "value");

        Assert.True(cache.Contains("a"));
    }

    [Fact]
    public void Contains_MissingKey_ReturnsFalse()
    {
        using var cache = new InMemoryCache<string, string>(10);

        Assert.False(cache.Contains("missing"));
    }

    [Fact]
    public void Remove_ExistingKey_ReturnsTrue()
    {
        using var cache = new InMemoryCache<string, int>(10);
        cache.Set("key", 1);

        Assert.True(cache.Remove("key"));
        Assert.False(cache.Contains("key"));
    }

    [Fact]
    public void Remove_MissingKey_ReturnsFalse()
    {
        using var cache = new InMemoryCache<string, int>(10);

        Assert.False(cache.Remove("missing"));
    }

    [Fact]
    public void Count_ReflectsNumberOfEntries()
    {
        using var cache = new InMemoryCache<int, string>(10);

        Assert.Equal(0, cache.Count);
        cache.Set(1, "a");
        Assert.Equal(1, cache.Count);
        cache.Set(2, "b");
        Assert.Equal(2, cache.Count);
        cache.Remove(1);
        Assert.Equal(1, cache.Count);
    }

    [Fact]
    public void Set_AtCapacity_EvictsLruEntry()
    {
        using var cache = new InMemoryCache<int, string>(2);
        cache.Set(1, "a");
        cache.Set(2, "b");

        cache.Set(3, "c"); // evicts key 1 (LRU)

        Assert.Equal(2, cache.Count);
        Assert.False(cache.Contains(1));
        Assert.True(cache.Contains(2));
        Assert.True(cache.Contains(3));
    }

    [Fact]
    public void Set_UpdateExistingKey_DoesNotEvict()
    {
        using var cache = new InMemoryCache<int, string>(2);
        cache.Set(1, "a");
        cache.Set(2, "b");

        cache.Set(1, "updated"); // update, not new entry

        Assert.Equal(2, cache.Count);
        cache.TryGet(1, out var val);
        Assert.Equal("updated", val);
    }

    [Fact]
    public void Constructor_ZeroCapacity_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new InMemoryCache<int, int>(0));
    }

    [Fact]
    public void Constructor_NegativeCapacity_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new InMemoryCache<int, int>(-1));
    }

    [Fact]
    public void Dispose_ThenTryGet_ThrowsObjectDisposedException()
    {
        var cache = new InMemoryCache<int, int>(10);
        cache.Dispose();

        Assert.Throws<ObjectDisposedException>(() => cache.TryGet(1, out _));
    }

    [Fact]
    public void Dispose_ThenSet_ThrowsObjectDisposedException()
    {
        var cache = new InMemoryCache<int, int>(10);
        cache.Dispose();

        Assert.Throws<ObjectDisposedException>(() => cache.Set(1, 1));
    }

    [Fact]
    public void Dispose_ThenContains_ThrowsObjectDisposedException()
    {
        var cache = new InMemoryCache<int, int>(10);
        cache.Dispose();

        Assert.Throws<ObjectDisposedException>(() => cache.Contains(1));
    }

    [Fact]
    public void Dispose_CalledTwice_DoesNotThrow()
    {
        var cache = new InMemoryCache<int, int>(10);
        cache.Dispose();
        cache.Dispose(); // should not throw
    }
}

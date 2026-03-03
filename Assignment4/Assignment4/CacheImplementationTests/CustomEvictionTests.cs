using CacheImplementation;
using NSubstitute;

namespace CacheImplementationTests;

public class CustomEvictionTests
{
    [Fact]
    public void InMemoryCache_WithCustomStrategy_UsesCustomEvict()
    {
        var strategy = Substitute.For<IEviction<int>>();
        // When asked to evict, return key 1 (simulating FIFO: first added)
        strategy.Evict().Returns(1);

        using var cache = new InMemoryCache<int, string>(2, strategy);
        cache.Set(1, "first");
        cache.Set(2, "second");

        // This should trigger eviction via custom strategy
        cache.Set(3, "third");

        strategy.Received(1).Evict();
    }

    [Fact]
    public void InMemoryCache_WithCustomStrategy_CallsOnAdded()
    {
        var strategy = Substitute.For<IEviction<int>>();
        using var cache = new InMemoryCache<int, string>(10, strategy);

        cache.Set(42, "value");

        strategy.Received(1).OnAdded(42);
    }

    [Fact]
    public void InMemoryCache_WithCustomStrategy_CallsOnAccessedOnTryGet()
    {
        var strategy = Substitute.For<IEviction<int>>();
        using var cache = new InMemoryCache<int, string>(10, strategy);
        cache.Set(1, "value");

        cache.TryGet(1, out _);

        strategy.Received().OnAccessed(1);
    }

    [Fact]
    public void InMemoryCache_WithCustomStrategy_CallsOnRemovedOnRemove()
    {
        var strategy = Substitute.For<IEviction<int>>();
        using var cache = new InMemoryCache<int, string>(10, strategy);
        cache.Set(1, "value");

        cache.Remove(1);

        strategy.Received(1).OnRemoved(1);
    }

    [Fact]
    public void InMemoryCache_NullStrategy_DefaultsToLru()
    {
        // Passing null should not throw; defaults to LruEvictionStrategy
        using var cache = new InMemoryCache<int, string>(2, null);

        cache.Set(1, "a");
        cache.Set(2, "b");
        cache.Set(3, "c"); // evicts 1 (LRU default)

        Assert.False(cache.Contains(1));
        Assert.True(cache.Contains(3));
    }
}

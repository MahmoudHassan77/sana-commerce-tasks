using CacheImplementation;

namespace CacheImplementationTests;

public class LruEvictionTests
{
    [Fact]
    public void Evict_ReturnsLeastRecentlyAddedKey()
    {
        var strategy = new LruEviction<string>();
        strategy.OnAdded("a");
        strategy.OnAdded("b");
        strategy.OnAdded("c");

        var evicted = strategy.Evict();

        Assert.Equal("a", evicted);
    }

    [Fact]
    public void OnAccessed_PromotesToMru_ChangesEvictionOrder()
    {
        var strategy = new LruEviction<string>();
        strategy.OnAdded("a");
        strategy.OnAdded("b");
        strategy.OnAdded("c");

        strategy.OnAccessed("a"); // "a" moves to MRU

        var evicted = strategy.Evict();
        Assert.Equal("b", evicted); // "b" is now LRU
    }

    [Fact]
    public void OnRemoved_CleansUpKey()
    {
        var strategy = new LruEviction<string>();
        strategy.OnAdded("a");
        strategy.OnAdded("b");

        strategy.OnRemoved("a");

        var evicted = strategy.Evict();
        Assert.Equal("b", evicted);
    }

    [Fact]
    public void Evict_OnEmpty_ThrowsInvalidOperationException()
    {
        var strategy = new LruEviction<int>();

        Assert.Throws<InvalidOperationException>(() => strategy.Evict());
    }

    [Fact]
    public void OnAdded_DuplicateKey_ThrowsInvalidOperationException()
    {
        var strategy = new LruEviction<string>();
        strategy.OnAdded("a");

        Assert.Throws<InvalidOperationException>(() => strategy.OnAdded("a"));
    }

    [Fact]
    public void MultipleEvictions_ReturnKeysInLruOrder()
    {
        var strategy = new LruEviction<int>();
        strategy.OnAdded(1);
        strategy.OnAdded(2);
        strategy.OnAdded(3);

        Assert.Equal(1, strategy.Evict());
        Assert.Equal(2, strategy.Evict());
        Assert.Equal(3, strategy.Evict());
    }

    [Fact]
    public void OnAccessed_UnknownKey_DoesNotThrow()
    {
        var strategy = new LruEviction<string>();

        // Accessing a key not in the strategy is a no-op
        strategy.OnAccessed("unknown");
    }
}

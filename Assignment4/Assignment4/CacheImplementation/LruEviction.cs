namespace CacheImplementation;

public class LruEviction<TKey> : IEviction<TKey> where TKey : notnull
{
    private readonly LinkedList<TKey> _order = new();
    private readonly Dictionary<TKey, LinkedListNode<TKey>> _nodes = new();

    public void OnAccessed(TKey key)
    {
        if (_nodes.TryGetValue(key, out var node))
        {
            _order.Remove(node);
            _order.AddFirst(node);
        }
    }

    public void OnAdded(TKey key)
    {
        if (_nodes.ContainsKey(key))
            throw new InvalidOperationException(
                $"Key '{key}' already exists in the eviction strategy. Use OnAccessed for existing keys.");

        var node = _order.AddFirst(key);
        _nodes[key] = node;
    }

    public void OnRemoved(TKey key)
    {
        if (_nodes.TryGetValue(key, out var node))
        {
            _order.Remove(node);
            _nodes.Remove(key);
        }
    }

    public TKey Evict()
    {
        var last = _order.Last
                   ?? throw new InvalidOperationException("Cannot evict from an empty cache.");

        var key = last.Value;
        _order.RemoveLast();
        _nodes.Remove(key);
        return key;
    }
}

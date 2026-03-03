namespace CacheImplementation;

public interface ICache<TKey, TValue> where TKey : notnull
{
    bool TryGet(TKey key, out TValue? value);
    void Set(TKey key, TValue value);
    bool Contains(TKey key);
    bool Remove(TKey key);
    int Count { get; }
}

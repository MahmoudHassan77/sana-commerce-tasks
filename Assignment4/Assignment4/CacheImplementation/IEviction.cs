namespace CacheImplementation;

public interface IEviction<TKey> where TKey : notnull
{
    void OnAccessed(TKey key);
    void OnAdded(TKey key);
    void OnRemoved(TKey key);
    TKey Evict();
}

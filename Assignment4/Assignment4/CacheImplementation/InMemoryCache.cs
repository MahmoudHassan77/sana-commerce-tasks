namespace CacheImplementation;

public class InMemoryCache<TKey, TValue> : ICache<TKey, TValue>, IDisposable
    where TKey : notnull
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, TValue> _store;
    private readonly IEviction<TKey> _eviction;
    private readonly ReaderWriterLockSlim _lock = new();
    private bool _disposed;
    
    public InMemoryCache(int capacity, IEviction<TKey>? evictionStrategy = null)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than 0.");

        _capacity = capacity;
        _store = new Dictionary<TKey, TValue>(capacity);
        _eviction = evictionStrategy ?? new LruEviction<TKey>();
    }

    public int Count
    {
        get
        {
            ThrowIfDisposed();
            _lock.EnterReadLock();
            try
            {
                return _store.Count;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    public bool TryGet(TKey key, out TValue? value)
    {
        ThrowIfDisposed();
        
        _lock.EnterReadLock();
        try
        {
            if (!_store.TryGetValue(key, out value))
            {
                value = default;
                return false;
            }
        }
        finally
        {
            _lock.ExitReadLock();
        }

        _lock.EnterWriteLock();
        try
        {
            if (_store.ContainsKey(key))
            {
                _eviction.OnAccessed(key);
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        return true;
    }

    public void Set(TKey key, TValue value)
    {
        ThrowIfDisposed();
        _lock.EnterWriteLock();
        try
        {
            if (_store.ContainsKey(key))
            {
                _store[key] = value;
                _eviction.OnAccessed(key);
                return;
            }

            if (_store.Count >= _capacity)
            {
                var evictedKey = _eviction.Evict();
                _store.Remove(evictedKey);
            }

            _store[key] = value;
            _eviction.OnAdded(key);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public bool Contains(TKey key)
    {
        ThrowIfDisposed();
        _lock.EnterReadLock();
        try
        {
            return _store.ContainsKey(key);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public bool Remove(TKey key)
    {
        ThrowIfDisposed();
        _lock.EnterWriteLock();
        try
        {
            if (_store.Remove(key))
            {
                _eviction.OnRemoved(key);
                return true;
            }
            return false;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _lock.Dispose();
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }
}

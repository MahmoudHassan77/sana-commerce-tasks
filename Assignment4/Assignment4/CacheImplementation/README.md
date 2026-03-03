# Assignment 4: In-Memory Cache with LRU Eviction

A thread-safe, high-performance in-memory cache in C# with a pluggable eviction strategy.

## Design Decisions & Patterns

- **Strategy Pattern**: Eviction behaviour is abstracted behind `IEvictionStrategy<TKey>`. The default is `LruEvictionStrategy`, but any custom strategy can be injected via the constructor.
- **O(1) LRU via Dictionary + LinkedList**: `Dictionary<TKey, TValue>` provides O(1) key lookup. `LinkedList<TKey>` in the eviction strategy maintains access order with O(1) move-to-front and remove-last operations.
- **`ReaderWriterLockSlim`**: Allows concurrent reads (via read locks) while serializing writes. `TryGet` uses a read lock for lookup, then upgrades to a write lock only when updating eviction order.
- **Dispose pattern**: `InMemoryCache` implements `IDisposable` with a `_disposed` guard — all public methods throw `ObjectDisposedException` after disposal.
- **Constructor validation**: Capacity must be > 0; `ArgumentOutOfRangeException` thrown otherwise.

## Architecture

```
┌─────────────────────┐
│   InMemoryCache     │ ← ICache<TKey, TValue>
│   (thread-safe)     │
│                     │
│   Dictionary + Lock │
│         │           │
│         ▼           │
│  IEvictionStrategy  │ ← pluggable
│         │           │
│    ┌────┴────┐      │
│    │  LRU    │      │ ← default: LinkedList-based
│    │  FIFO   │      │ ← custom: user-provided
│    │  LFU    │      │ ← custom: user-provided
│    └─────────┘      │
└─────────────────────┘
```

## File Structure

```
Assignment4.CacheImpl/
├── ICache.cs                  # Cache interface: TryGet, Set, Contains, Remove, Count
├── IEvictionStrategy.cs       # Strategy interface: OnAccessed, OnAdded, OnRemoved, Evict
├── LruEvictionStrategy.cs     # Default LRU implementation
├── InMemoryCache.cs           # Thread-safe cache with pluggable eviction
├── Services/
│   └── JsonPlaceholderClient.cs  # HTTP client for demo data
└── Program.cs                 # Demo: cache miss/hit, eviction, concurrency
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Build & Run

```bash
cd src/Assignment4.CacheImpl
dotnet run
```

**Expected output**: Console demonstrates cache miss (fetch from API), cache hit, LRU eviction at capacity, concurrent multi-threaded access, and pluggable strategy info.

## Run Tests

```bash
cd src/Assignment4.CacheImpl.Tests
dotnet test
```


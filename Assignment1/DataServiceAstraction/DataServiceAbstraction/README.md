# Assignment 1: Data Service Abstraction


## Design Decisions & Patterns

- **Decorator Pattern**: `DataServiceCaching` and `DataServiceLogger` both implement `IDataService` and wrap an inner `IDataService`. This allows behaviours to be composed in any order without modifying existing classes.
- **Single Responsibility (SRP)**: Each class has one job — `DataService` reads files, `DataServiceCaching` caches, `DataServiceLogger` logs.
- **Open/Closed (OCP)**: New behaviours (retry, validation) can be added as new decorators without editing existing code.
- **Dependency Inversion (DIP)**: All decorators depend on `IDataService`, not concrete implementations.
- **Thread-safe caching**: Uses `Lazy<T>` for safe lazy initialization.

## File Structure

```
DataServiceAbstraction/
    - IDataService.cs                  
    - DataService.cs              
    - DataServiceCaching.cs   
    - DataServiceLogger.cs   
    - Program.cs                       
    - data.txt              
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Build & Run

```bash
cd DataServiceAbstraction
dotnet build
dotnet run
```

**Expected output**:

- The first call prints "First call it will go through logging - caching - dataService - file" in green, followed by log messages from DataServiceLogger showing that lines were read from data.txt via DataServiceCaching and DataService.
- The second call prints "Second call it will go through logging - caching" in green (cache hit and no file read).
- A short delay between calls ensures console output is clearly separated because the console logger is asynchronous.
- The decorator composition (Logging → Caching → DataService) controls the order in which logging and caching occur.


## Run Tests

```bash
cd DataServiceAbstractionTests
dotnet test
```

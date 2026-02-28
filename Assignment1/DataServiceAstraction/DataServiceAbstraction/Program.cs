using DataServiceAbstraction;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});

var logger = loggerFactory.CreateLogger<Program>();

IDataService dataService = new DataServiceLogger(new DataServiceCaching(new DataService("data.txt")), logger);

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("First call it will go through logging - caching - dataService - file");
Console.ResetColor();
dataService.GetLines();

// Adding a short delay to make console output easier to read: separates first and second call outputs because the Microsoft Console Logger is asynchronous
Thread.Sleep(300);

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Second call it will go through logging - caching");
Console.ResetColor();
dataService.GetLines();
using Microsoft.Extensions.Logging;
namespace DataServiceAbstraction;

public class DataServiceLogger(IDataService dataService, ILogger logger) : IDataService
{
    private readonly IDataService _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public IEnumerable<string> GetLines()
    {
        var lines = _dataService.GetLines().ToList();
        foreach (var line in lines)
        {
            _logger.LogInformation(line);
        }
        return lines;
    }
}
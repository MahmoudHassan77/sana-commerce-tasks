namespace DataServiceAbstraction;

public class DataServiceCaching : IDataService
{
    private readonly Lazy<IReadOnlyList<string>> _cachedLines;

    public DataServiceCaching(IDataService dataService)
    {
        if (dataService is null)
            throw new ArgumentNullException(nameof(dataService));
        _cachedLines = new Lazy<IReadOnlyList<string>>(
            () => dataService.GetLines().ToList().AsReadOnly());
    }

    public IEnumerable<string> GetLines()
    {
        return _cachedLines.Value;
    }
}

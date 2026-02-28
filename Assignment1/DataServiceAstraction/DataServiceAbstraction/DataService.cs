namespace DataServiceAbstraction;

public class DataService : IDataService
{
    private readonly string _filePath;

    public DataService(string filePath)
    {
        if(filePath is null)
            throw new ArgumentNullException(nameof(filePath));
        if (!File.Exists(filePath))
            throw new FileNotFoundException(_filePath);
        _filePath = filePath;
    }
    public IEnumerable<string> GetLines()
    {
        return File.ReadAllLines(_filePath);
    }
}
namespace DataServiceAbstraction;

public interface IDataService
{
    IEnumerable<string> GetLines();
}
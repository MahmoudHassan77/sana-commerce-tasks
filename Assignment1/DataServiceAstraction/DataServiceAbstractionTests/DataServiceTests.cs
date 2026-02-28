using DataServiceAbstraction;

namespace DataServiceAbstractionTests;

public class DataServiceTests: IDisposable
{
    private readonly string _tempFile;

    public DataServiceTests()
    {
        _tempFile = Path.GetTempFileName();
        File.WriteAllLines(_tempFile, ["Line one", "Line two", "Line three"]);
    }

    public void Dispose()
    {
        if (File.Exists(_tempFile))
            File.Delete(_tempFile);
    }

    [Fact]
    public void GetLines_ReturnsAllLinesInOrder()
    {
        var service = new DataService(_tempFile);

        var lines = service.GetLines().ToList();

        Assert.Equal(3, lines.Count);
        Assert.Equal("Line one", lines[0]);
        Assert.Equal("Line two", lines[1]);
        Assert.Equal("Line three", lines[2]);
    }

    [Fact]
    public void GetLines_EmptyFile_ReturnsEmptyCollection()
    {
        File.WriteAllText(_tempFile, "");
        var service = new DataService(_tempFile);

        var lines = service.GetLines().ToList();

        Assert.Empty(lines);
    }

    [Fact]
    public void Constructor_NullPath_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new DataService(null!));
    }

    [Fact]
    public void Constructor_MissingFile_ThrowsFileNotFoundException()
    {
        Assert.Throws<FileNotFoundException>(
            () => new DataService("/nonexistent/path/data.txt"));
    }
}
using DataServiceAbstraction;
using NSubstitute;

namespace DataServiceAbstractionTests;

public class DataServiceCachingTests
{
    [Fact]
    public void GetLines_FirstCall_DelegatesToDataServiceService()
    {
        var dataService = Substitute.For<IDataService>();
        dataService.GetLines().Returns(["A", "B"]);
        var sut = new DataServiceCaching(dataService);

        var result = sut.GetLines().ToList();

        Assert.Equal(["A", "B"], result);
        dataService.Received(1).GetLines();
    }

    [Fact]
    public void GetLines_SecondCall_ReturnsCachedWithoutCallingDataServiceAgain()
    {
        var dataService = Substitute.For<IDataService>();
        dataService.GetLines().Returns(["A", "B"]);
        var sut = new DataServiceCaching(dataService);

        _ = sut.GetLines().ToList();
        var second = sut.GetLines().ToList();

        Assert.Equal(["A", "B"], second);
        dataService.Received(1).GetLines();
    }

    [Fact]
    public void GetLines_CachedResult_IsIdenticalReference()
    {
        var dataService = Substitute.For<IDataService>();
        dataService.GetLines().Returns(["X"]);
        var sut = new DataServiceCaching(dataService);

        var first = sut.GetLines();
        var second = sut.GetLines();

        Assert.Same(first, second);
    }

    [Fact]
    public void Constructor_NullDataService_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new DataServiceCaching(null!));
    }
}
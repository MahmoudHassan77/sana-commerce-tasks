using DataServiceAbstraction;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace DataServiceAbstractionTests;

public class DataServiceLoggerTests
{
    [Fact]
    public void GetLines_ReturnsDataFromDataServiceUnchanged()
    {
        var dataService = Substitute.For<IDataService>();
        dataService.GetLines().Returns(["Hello", "World"]);
        var logger = Substitute.For<ILogger>();
        var sut = new DataServiceLogger(dataService, logger);

        var result = sut.GetLines().ToList();

        Assert.Equal(["Hello", "World"], result);
    }

    [Fact]
    public void GetLines_LogsEachReturnedLine()
    {
        var dataService = Substitute.For<IDataService>();
        dataService.GetLines().Returns(["A", "B", "C"]);
        var logger = Substitute.For<ILogger>();
        var sut = new DataServiceLogger(dataService, logger);

        _ = sut.GetLines().ToList();
        
        logger.ReceivedWithAnyArgs(3).Log(
            default, default, default(object), default, default!);
    }

    [Fact]
    public void GetLines_EmptyResult_NoLogCalls()
    {
        var dataService = Substitute.For<IDataService>();
        dataService.GetLines().Returns(Array.Empty<string>());
        var logger = Substitute.For<ILogger>();
        var sut = new DataServiceLogger(dataService, logger);

        _ = sut.GetLines().ToList();

        logger.DidNotReceiveWithAnyArgs().Log(
            default, default, default(object), default, default!);
    }

    [Fact]
    public void Constructor_NullDataService_ThrowsArgumentNullException()
    {
        var logger = Substitute.For<ILogger>();
        Assert.Throws<ArgumentNullException>(
            () => new DataServiceLogger(null!, logger));
    }

    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        var dataService = Substitute.For<IDataService>();
        Assert.Throws<ArgumentNullException>(
            () => new DataServiceLogger(dataService, null!));
    }
}
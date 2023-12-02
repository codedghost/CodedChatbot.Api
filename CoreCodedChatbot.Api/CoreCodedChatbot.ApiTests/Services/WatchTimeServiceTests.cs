using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.WatchTime;
using CoreCodedChatbot.ApiApplication.Services;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Services;

[TestFixture]
public class WatchTimeServiceTests
{
    private Mock<IGetWatchTimeRepository> _getWatchTimeRespository;

    private WatchTimeService _subject;

    [SetUp]
    public void Setup()
    {
        _getWatchTimeRespository = new Mock<IGetWatchTimeRepository>();

        _subject = new WatchTimeService(_getWatchTimeRespository.Object);
    }

    [Test, AutoData]
    public async Task SuccessWhen_GetWatchTimeMethodIsCalled(string username)
    {
        await _subject.GetWatchTime(username);

        _getWatchTimeRespository.Verify(r => r.Get(username), Times.Once);
    }
}
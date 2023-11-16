using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.Config;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist;

[TestFixture]
public class ProcessRegularSongRequestCommandTests
{
    private Mock<IConfigService> _configService;
    private Mock<ICheckUserHasMaxRegularsInQueueQuery> _checkUserHasMaxRegularsInQueueQuery;
    private Mock<IAddRequestRepository> _addRequestRepository;

    private ProcessRegularSongRequestCommand _subject;

    [SetUp]
    public void SetUp()
    {
        var maxRegulars = 1;

        _configService = new Mock<IConfigService>();
        _checkUserHasMaxRegularsInQueueQuery = new Mock<ICheckUserHasMaxRegularsInQueueQuery>();
        _addRequestRepository = new Mock<IAddRequestRepository>();

        _configService.Setup(c => c.Get<int>("MaxRegularSongsPerUser")).Returns(maxRegulars);
        _addRequestRepository.Setup(a => a.AddRequest(It.IsAny<string>(), It.IsAny<string>(), false, false, It.IsAny<int>()))
            .Returns(new AddSongResult
            {
                AddRequestResult = AddRequestResult.Success,
                MaximumRegularRequests = maxRegulars
            });
    }

    private void SetUserHasMaxRegulars(bool hasMaxRegulars)
    {
        _checkUserHasMaxRegularsInQueueQuery.Setup(c => c.UserHasMaxRegularsInQueue(It.IsAny<string>()))
            .Returns(hasMaxRegulars);
    }

    private void SetUpSubject()
    {
        _subject = new ProcessRegularSongRequestCommand(_configService.Object,
            _checkUserHasMaxRegularsInQueueQuery.Object,
            _addRequestRepository.Object);
    }

    [Test]
    public void SuccessWhen_UserHasNoRegulars()
    {
        SetUserHasMaxRegulars(false);
        SetUpSubject();

        var result = _subject.Process("Username", "Request Text", It.IsAny<int>());

        Assert.AreEqual(AddRequestResult.Success, result.AddRequestResult);
    }

    [Test]
    public void FailureWhen_UserHasMaxRegulars()
    {
        SetUserHasMaxRegulars(true);
        SetUpSubject();

        var maxRequests = _configService.Object.Get<int>("MaxRegularSongsPerUser");

        var result = _subject.Process("Username", "Request Text", It.IsAny<int>());

        Assert.AreEqual(AddRequestResult.MaximumRegularRequests, result.AddRequestResult);
        Assert.AreEqual(maxRequests, result.MaximumRegularRequests);
    }
}
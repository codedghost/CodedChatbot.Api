using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist;

[TestFixture]
public class UpdatePlaylistStateCommandTests
{
    private Mock<ISetOrCreateSettingRepository> _setOrCreateSettingRepository;

    private UpdatePlaylistStateCommand _subject;

    [SetUp]
    public void SetUp()
    {
        _setOrCreateSettingRepository = new Mock<ISetOrCreateSettingRepository>();

        _subject = new UpdatePlaylistStateCommand(_setOrCreateSettingRepository.Object);
    }

    [TestCase(PlaylistState.Open, "PlaylistStatus", "Open", true, TestName = "SuccessWhen_ValidPlaylistState")]
    [TestCase(PlaylistState.Closed, "PlaylistStatus", "Closed", true, TestName = "SuccessWhen_ValidPlaylistState")]
    [TestCase(PlaylistState.VeryClosed, "PlaylistStatus", "VeryClosed", true, TestName = "SuccessWhen_ValidPlaylistState")]
    public void TestSuccess(PlaylistState state, string settingsKey, string settingsValue, bool expectedResult)
    {
        var result = _subject.UpdatePlaylistState(state);

        _setOrCreateSettingRepository.Verify(s => s.Set(settingsKey, settingsValue), Times.Once);

        Assert.AreEqual(expectedResult, result);
    }

    [TestCase(4, "PlaylistStatus", "Open", false, TestName = "FailureWhen_InvalidPlaylistState")]
    [TestCase(4, "PlaylistStatus", "Closed", false, TestName = "FailureWhen_InvalidPlaylistState")]
    [TestCase(4, "PlaylistStatus", "VeryClosed", false, TestName = "FailureWhen_InvalidPlaylistState")]
    public void TestFailure(PlaylistState state, string settingsKey, string settingsValue, bool expectedResult)
    {
        var result = _subject.UpdatePlaylistState(state);

        _setOrCreateSettingRepository.Verify(s => s.Set(settingsKey, settingsValue), Times.Never);

        Assert.AreEqual(expectedResult, result);
    }
}
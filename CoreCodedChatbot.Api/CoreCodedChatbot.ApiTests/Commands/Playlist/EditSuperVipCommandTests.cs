using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist;

[TestFixture]
public class EditSuperVipCommandTests
{
    private Mock<IEditSuperVipRequestRepository> _editSuperVipRequestRepository;

    private EditSuperVipCommand _subject;

    [SetUp]
    public void SetUp()
    {
        _editSuperVipRequestRepository = new Mock<IEditSuperVipRequestRepository>();

        _subject = new EditSuperVipCommand(_editSuperVipRequestRepository.Object);
    }

    [Test, AutoData]
    public void EnsureThat_RepositoryIsCalled(string username, string newText, int songRequestId, int songId)
    {
        _editSuperVipRequestRepository.Setup(e => e.Edit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .Returns(songRequestId);

        var result = _subject.Edit(username, newText, songId);

        _editSuperVipRequestRepository.Verify(e => e.Edit(username, newText,songId));

        Assert.AreEqual(songRequestId, result);
    }
}
using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist
{
    [TestFixture]
    public class ProcessSongRequestCommandTests
    {
        private Mock<IProcessRegularSongRequestCommand> _processRegularSongRequestCommand;
        private Mock<IProcessVipSongRequestCommand> _processVipSongRequestCommand;
        private Mock<IProcessSuperVipSongRequestCommand> _processSuperVipSongRequestCommand;

        private ProcessSongRequestCommand _subject;

        [SetUp]
        public void Setup()
        {
            _processRegularSongRequestCommand = new Mock<IProcessRegularSongRequestCommand>();
            _processVipSongRequestCommand = new Mock<IProcessVipSongRequestCommand>();
            _processSuperVipSongRequestCommand = new Mock<IProcessSuperVipSongRequestCommand>();

            _processRegularSongRequestCommand.Setup(p => p.Process(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new AddSongResult {AddRequestResult = AddRequestResult.Success});
            _processVipSongRequestCommand.Setup(p => p.Process(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AddSongResult { AddRequestResult = AddRequestResult.Success });
            _processSuperVipSongRequestCommand.Setup(p => p.Process(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AddSongResult { AddRequestResult = AddRequestResult.Success });

            _subject = new ProcessSongRequestCommand(_processRegularSongRequestCommand.Object,
                _processVipSongRequestCommand.Object,
                _processSuperVipSongRequestCommand.Object);
        }

        [TestCase("Username", "Request Text", SongRequestType.Regular, TestName = "SuccessWhen_RegularRequest")]
        [TestCase("Username", "Request Text", SongRequestType.Vip, TestName = "SuccessWhen_VipRequest")]
        [TestCase("Username", "Request Text", SongRequestType.SuperVip, TestName = "SuccessWhen_SuperVipRequest")]
        public async Task SuccessTest(string username, string requestText, SongRequestType requestType)
        {
            var result = await _subject.ProcessAddingSongRequest(username, requestText, requestType);

            Assert.AreEqual(AddRequestResult.Success, result.AddRequestResult);
        }

        [TestCase("Username", "Request Text", SongRequestType.Any, TestName = "ExceptionWhen_AnyRequestType")]
        public void ExceptionTest(string username, string requestText, SongRequestType requestType)
        { 
            Assert.ThrowsAsync<Exception>(async () => await _subject.ProcessAddingSongRequest(username, requestText, requestType));
        }
    }
}
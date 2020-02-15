using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist
{
    [TestFixture]
    public class AddSongRequestCommandTests
    {
        private Mock<IGetPlaylistStateQuery> _getPlaylistStateQuery;
        private Mock<IProcessSongRequestCommand> _processSongRequestCommand;

        private AddSongRequestCommand _subject;

        [SetUp]
        public void Setup()
        {
            _getPlaylistStateQuery = new Mock<IGetPlaylistStateQuery>();
            _processSongRequestCommand = new Mock<IProcessSongRequestCommand>();

            _processSongRequestCommand.Setup(p =>
                    p.ProcessAddingSongRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SongRequestType>()))
                .Returns(new AddSongResult
                {
                    AddRequestResult = AddRequestResult.Success
                });
        }

        [TestCase(PlaylistState.Open, "Test", "Test Song", SongRequestType.Regular, AddRequestResult.Success, TestName = "SuccessWhen_PlaylistIsOpen_UserNameProvided_SongProvided_RegularRequest")]
        [TestCase(PlaylistState.Open, "Test", "Test Song", SongRequestType.Vip, AddRequestResult.Success, TestName = "SuccessWhen_PlaylistIsOpen_UserNameProvided_SongProvided_VipRequest")]
        [TestCase(PlaylistState.Open, "Test", "Test Song", SongRequestType.SuperVip, AddRequestResult.Success, TestName = "SuccessWhen_PlaylistIsOpen_UserNameProvided_SongProvided_SuperVipRequest")]
        [TestCase(PlaylistState.Closed, "Test", "Test Song", SongRequestType.Regular, AddRequestResult.PlaylistClosed, TestName = "ClosedWhen_PlaylistIsClosed_UsernameProvided_SongProvided_RegularRequest")]
        [TestCase(PlaylistState.Closed, "Test", "Test Song", SongRequestType.Vip, AddRequestResult.Success, TestName = "SuccessWhen_PlaylistIsClosed_UserNameProvided_SongProvided_VipRequest")]
        [TestCase(PlaylistState.Closed, "Test", "Test Song", SongRequestType.SuperVip, AddRequestResult.Success, TestName = "SuccessWhen_PlaylistIsClosed_UserNameProvided_SongProvided_SuperVipRequest")]
        [TestCase(PlaylistState.VeryClosed, "Test", "Test Song", SongRequestType.Regular, AddRequestResult.PlaylistVeryClosed, TestName = "VeryClosedWhen_PlaylistIsVeryClosed_UsernameProvided_SongProvided_RegularRequest")]
        [TestCase(PlaylistState.VeryClosed, "Test", "Test Song", SongRequestType.Vip, AddRequestResult.PlaylistVeryClosed, TestName = "VeryClosedWhen_PlaylistIsVeryClosed_UserNameProvided_SongProvided_VipRequest")]
        [TestCase(PlaylistState.VeryClosed, "Test", "Test Song", SongRequestType.SuperVip, AddRequestResult.Success, TestName = "SuccessWhen_PlaylistIsVeryClosed_UserNameProvided_SongProvided_SuperVipRequest")]
        [TestCase(PlaylistState.Open, "", "Test Song", SongRequestType.Regular, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsOpen_UserNameNotProvided_SongProvided_RegularRequest")]
        [TestCase(PlaylistState.Open, "", "Test Song", SongRequestType.Vip, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsOpen_UserNameNotProvided_SongProvided_VipRequest")]
        [TestCase(PlaylistState.Open, "", "Test Song", SongRequestType.SuperVip, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsOpen_UserNameNotProvided_SongProvided_SuperVipRequest")]
        [TestCase(PlaylistState.Closed, "", "Test Song", SongRequestType.Regular, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsClosed_UsernameNotProvided_SongProvided_RegularRequest")]
        [TestCase(PlaylistState.Closed, "", "Test Song", SongRequestType.Vip, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsClosed_UserNameNotProvided_SongProvided_VipRequest")]
        [TestCase(PlaylistState.Closed, "", "Test Song", SongRequestType.SuperVip, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsClosed_UserNameNotProvided_SongProvided_SuperVipRequest")]
        [TestCase(PlaylistState.VeryClosed, "", "Test Song", SongRequestType.Regular, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsVeryClosed_UsernameNotProvided_SongProvided_RegularRequest")]
        [TestCase(PlaylistState.VeryClosed, "", "Test Song", SongRequestType.Vip, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsVeryClosed_UserNameNotProvided_SongProvided_VipRequest")]
        [TestCase(PlaylistState.VeryClosed, "", "Test Song", SongRequestType.SuperVip, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsVeryClosed_UserNameNotProvided_SongProvided_SuperVipRequest")]
        [TestCase(PlaylistState.Open, "Test", "Test Song", SongRequestType.Any, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsOpen_UserNameProvided_SongProvided_AnyRequest")]
        [TestCase(PlaylistState.Closed, "Test", "Test Song", SongRequestType.Any, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsClosed_UsernameProvided_SongProvided_AnyRequest")]
        [TestCase(PlaylistState.VeryClosed, "Test", "Test Song", SongRequestType.Any, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsVeryClosed_UsernameProvided_SongProvided_AnyRequest")]
        [TestCase(PlaylistState.Open, "", "Test Song", SongRequestType.Any, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsOpen_UserNameNotProvided_SongProvided_AnyRequest")]
        [TestCase(PlaylistState.Closed, "", "Test Song", SongRequestType.Any, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsClosed_UsernameNotProvided_SongProvided_AnyRequest")]
        [TestCase(PlaylistState.VeryClosed, "", "Test Song", SongRequestType.Any, AddRequestResult.UnSuccessful, TestName = "UnSuccessfulWhen_PlaylistIsVeryClosed_UsernameNotProvided_SongProvided_AnyRequest")]
        [TestCase(PlaylistState.Open, "Test", "", SongRequestType.Regular, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsOpen_UserNameProvided_SongProvided_RegularRequest")]
        [TestCase(PlaylistState.Open, "Test", "", SongRequestType.Vip, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsOpen_UserNameProvided_SongProvided_VipRequest")]
        [TestCase(PlaylistState.Open, "Test", "", SongRequestType.SuperVip, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsOpen_UserNameProvided_SongProvided_SuperVipRequest")]
        [TestCase(PlaylistState.Closed, "Test", "", SongRequestType.Regular, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsClosed_UsernameProvided_SongProvided_RegularRequest")]
        [TestCase(PlaylistState.Closed, "Test", "", SongRequestType.Vip, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsClosed_UserNameProvided_SongProvided_VipRequest")]
        [TestCase(PlaylistState.Closed, "Test", "", SongRequestType.SuperVip, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsClosed_UserNameProvided_SongProvided_SuperVipRequest")]
        [TestCase(PlaylistState.VeryClosed, "Test", "", SongRequestType.Regular, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsVeryClosed_UsernameProvided_SongProvided_RegularRequest")]
        [TestCase(PlaylistState.VeryClosed, "Test", "", SongRequestType.Vip, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsVeryClosed_UserNameProvided_SongProvided_VipRequest")]
        [TestCase(PlaylistState.VeryClosed, "Test", "", SongRequestType.SuperVip, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsVeryClosed_UserNameProvided_SongProvided_SuperVipRequest")]
        [TestCase(PlaylistState.Open, "Test", "", SongRequestType.Any, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsOpen_UserNameProvided_SongProvided_AnyRequest")]
        [TestCase(PlaylistState.Closed, "Test", "", SongRequestType.Any, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsClosed_UsernameProvided_SongProvided_AnyRequest")]
        [TestCase(PlaylistState.VeryClosed, "Test", "", SongRequestType.Any, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsVeryClosed_UsernameProvided_SongProvided_AnyRequest")]
        [TestCase(PlaylistState.Open, "", "", SongRequestType.Any, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsOpen_UserNameNotProvided_SongProvided_AnyRequest")]
        [TestCase(PlaylistState.Closed, "", "", SongRequestType.Any, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsClosed_UsernameNotProvided_SongProvided_AnyRequest")]
        [TestCase(PlaylistState.VeryClosed, "", "", SongRequestType.Any, AddRequestResult.NoRequestEntered, TestName = "NoRequestEnteredWhen_PlaylistIsVeryClosed_UsernameNotProvided_SongProvided_AnyRequest")]
        public void Test(PlaylistState state, string username, string requestText, SongRequestType requestType, AddRequestResult expectedResult)
        {
            // Arrange
            SetupTest(state);

            // Act
            var result = _subject.AddSongRequest(username, requestText, requestType);

            // Assert
            Assert.AreEqual(expectedResult, result.AddRequestResult);
        }


        #region Moq Setups

        private void SetPlaylistState(PlaylistState playlistState)
        {
            _getPlaylistStateQuery.Setup(p => p.GetPlaylistState()).Returns(playlistState);
        }

        private void SetupSubject()
        {
            _subject = new AddSongRequestCommand(_getPlaylistStateQuery.Object, _processSongRequestCommand.Object);
        }

        private void SetupTest(PlaylistState state)
        {
            SetPlaylistState(state);

            SetupSubject();
        }

        #endregion
    }
}
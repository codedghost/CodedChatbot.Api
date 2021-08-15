using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class ProcessSongRequestCommand : IProcessSongRequestCommand
    {
        private readonly IProcessRegularSongRequestCommand _processRegularSongRequestCommand;
        private readonly IProcessVipSongRequestCommand _processVipSongRequestCommand;
        private readonly IProcessSuperVipSongRequestCommand _processSuperVipSongRequestCommand;

        public ProcessSongRequestCommand(
            IProcessRegularSongRequestCommand processRegularSongRequestCommand,
            IProcessVipSongRequestCommand processVipSongRequestCommand,
            IProcessSuperVipSongRequestCommand processSuperVipSongRequestCommand
            )
        {
            _processRegularSongRequestCommand = processRegularSongRequestCommand;
            _processVipSongRequestCommand = processVipSongRequestCommand;
            _processSuperVipSongRequestCommand = processSuperVipSongRequestCommand;
        }

        public async Task<AddSongResult> ProcessAddingSongRequest(string username, string requestText,
            SongRequestType songRequestType)
        {
            switch (songRequestType)
            {
                case SongRequestType.SuperVip:
                    return await _processSuperVipSongRequestCommand.Process(username, requestText).ConfigureAwait(false);
                case SongRequestType.Vip:
                    return await _processVipSongRequestCommand.Process(username, requestText).ConfigureAwait(false);
                case SongRequestType.Regular:
                    return _processRegularSongRequestCommand.Process(username, requestText);
                default:
                    throw new Exception(
                        $"Requested a new song of type Any, Username: {username}, RequestText: {requestText}");
            }
        }



    }
}
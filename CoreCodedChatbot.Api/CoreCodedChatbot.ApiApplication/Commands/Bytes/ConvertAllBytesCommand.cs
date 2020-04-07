using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Bytes
{
    public class ConvertAllBytesCommand : IConvertAllBytesCommand
    {
        private readonly IGetUserByteCountRepository _getUserByteCountRepository;
        private readonly IConvertBytesCommand _convertBytesCommand;
        private readonly IConfigService _configService;

        public ConvertAllBytesCommand(
            IGetUserByteCountRepository getUserByteCountRepository,
            IConvertBytesCommand convertBytesCommand,
            IConfigService configService
            )
        {
            _getUserByteCountRepository = getUserByteCountRepository;
            _convertBytesCommand = convertBytesCommand;
            _configService = configService;
        }

        public int Convert(string username)
        {
            var conversionAmount = _configService.Get<int>("BytesToVip");

            var usersBytes = _getUserByteCountRepository.Get(username, conversionAmount);

            var closestWholeBytes = (int) Math.Floor(usersBytes);

            var convertedBytes = _convertBytesCommand.Convert(username, closestWholeBytes);

            return convertedBytes;
        }
    }
}
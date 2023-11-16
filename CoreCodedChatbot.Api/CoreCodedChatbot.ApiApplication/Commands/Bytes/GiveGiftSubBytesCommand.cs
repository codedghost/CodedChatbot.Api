using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Bytes;

public class GiveGiftSubBytesCommand : IGiveGiftSubBytesCommand
{
    private readonly IGiveGiftSubBytesRepository _giveGiftSubBytesRepository;
    private readonly IConfigService _configService;

    public GiveGiftSubBytesCommand(
        IGiveGiftSubBytesRepository giveGiftSubBytesRepository,
        IConfigService configService)
    {
        _giveGiftSubBytesRepository = giveGiftSubBytesRepository;
        _configService = configService;
    }

    public void Give(string username)
    {
        var conversionAmount = _configService.Get<int>("BytesToVip");

        _giveGiftSubBytesRepository.Give(username, conversionAmount);
    }
}
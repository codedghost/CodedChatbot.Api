using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Bytes;

public class ConvertBytesCommand : IConvertBytesCommand
{
    private readonly IConvertBytesRepository _convertBytesRepository;
    private readonly IConfigService _configService;

    public ConvertBytesCommand(
        IConvertBytesRepository convertBytesRepository,
        IConfigService configService
    )
    {
        _convertBytesRepository = convertBytesRepository;
        _configService = configService;
    }

    public int Convert(string username, int bytesToConvert)
    {
        var conversionAmount = _configService.Get<int>("BytesToVip");

        var convertedTokens = _convertBytesRepository.Convert(username, bytesToConvert, conversionAmount);

        return convertedTokens;
    }
}
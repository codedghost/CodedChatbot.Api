using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Queries.Bytes;

public class GetUserByteCountQuery : IGetUserByteCountQuery
{
    private readonly IGetUserByteCountRepository _getUserByteCountRepository;
    private readonly IConfigService _configService;

    public GetUserByteCountQuery(
        IGetUserByteCountRepository getUserByteCountRepository,
        IConfigService configService
    )
    {
        _getUserByteCountRepository = getUserByteCountRepository;
        _configService = configService;
    }

    public string Get(string username)
    {
        var conversionAmount = _configService.Get<int>("BytesToVip");
        var bytes = _getUserByteCountRepository.Get(username, conversionAmount);

        return bytes.ToString("n3");
    }
}
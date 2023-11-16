using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.ApiApplication.Queries.Vip;

public class CheckUserHasVipsQuery : ICheckUserHasVipsQuery
{
    private readonly IGetUsersVipCountRepository _getUsersVipCountRepository;

    public CheckUserHasVipsQuery(
        IGetUsersVipCountRepository getUsersVipCountRepository
    )
    {
        _getUsersVipCountRepository = getUsersVipCountRepository;
    }

    public bool CheckUserHasVips(string username, int vipsToCheck)
    {
        var userVips = _getUsersVipCountRepository.GetVips(username);

        return userVips >= vipsToCheck;
    }
}
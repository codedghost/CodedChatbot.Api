using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.ApiApplication.Queries.Vip;

public class GetUserVipCountQuery : IGetUserVipCountQuery
{
    private readonly IGetUsersVipCountRepository _getUsersVipCountRepository;

    public GetUserVipCountQuery(IGetUsersVipCountRepository getUsersVipCountRepository)
    {
        _getUsersVipCountRepository = getUsersVipCountRepository;
    }

    public int Get(string username)
    {
        return _getUsersVipCountRepository.GetVips(username);
    }
}
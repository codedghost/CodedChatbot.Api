namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip
{
    public interface IGetUserVipCountQuery
    {
        int Get(string username);
    }
}
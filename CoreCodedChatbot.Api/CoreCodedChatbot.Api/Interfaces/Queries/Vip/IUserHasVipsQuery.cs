namespace CoreCodedChatbot.Api.Interfaces.Queries.Vip
{
    public interface IUserHasVipsQuery
    {
        bool UserHasVips(string username, int vips);
    }
}
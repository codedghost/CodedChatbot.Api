namespace CoreCodedChatbot.Api.Interfaces.Repositories.Vip
{
    public interface IUserHasVipsRepository
    {
        bool HasVips(string username, int vips);
    }
}
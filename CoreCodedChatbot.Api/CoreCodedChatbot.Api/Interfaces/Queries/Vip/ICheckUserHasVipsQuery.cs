namespace CoreCodedChatbot.Api.Interfaces.Queries.Vip
{
    public interface ICheckUserHasVipsQuery
    {
        bool CheckUserHasVips(string username, int vipsToCheck);
    }
}
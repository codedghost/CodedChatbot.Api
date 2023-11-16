namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;

public interface ICheckUserHasVipsQuery
{
    bool CheckUserHasVips(string username, int vipsToCheck);
}
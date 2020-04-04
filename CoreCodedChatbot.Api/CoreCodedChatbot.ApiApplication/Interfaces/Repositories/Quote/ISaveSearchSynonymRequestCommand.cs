namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote
{
    public interface ISaveSearchSynonymRequestCommand
    {
        bool Save(string synonymRequest, string username);
    }
}
namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote
{
    public interface ISaveSearchSynonymRequestRepository
    {
        void Save(string synonymRequest, string username);
    }
}
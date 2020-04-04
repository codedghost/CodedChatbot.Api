namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search
{
    public interface ISaveSearchSynonymRequestRepository
    {
        void Save(string synonymRequest, string username);
    }
}
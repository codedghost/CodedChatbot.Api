namespace CoreCodedChatbot.Api.Interfaces.Repositories
{
    public interface ISaveSearchSynonymRequestRepository
    {
        void Save(string synonymRequest, string username);
    }
}
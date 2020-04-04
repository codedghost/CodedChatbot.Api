namespace CoreCodedChatbot.Api.Interfaces.Repositories
{
    public interface ISaveSearchSynonymRequestCommand
    {
        bool Save(string synonymRequest, string username);
    }
}
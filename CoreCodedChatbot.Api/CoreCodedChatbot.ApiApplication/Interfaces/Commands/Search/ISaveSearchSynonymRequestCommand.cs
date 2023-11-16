namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Search;

public interface ISaveSearchSynonymRequestCommand
{
    bool Save(string synonymRequest, string username);
}
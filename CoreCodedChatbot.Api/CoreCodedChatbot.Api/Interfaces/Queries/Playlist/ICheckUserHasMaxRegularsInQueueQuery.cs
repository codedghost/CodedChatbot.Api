namespace CoreCodedChatbot.Api.Interfaces.Queries.Playlist
{
    public interface ICheckUserHasMaxRegularsInQueueQuery
    {
        bool UserHasMaxRegularsInQueue(string username);
    }
}
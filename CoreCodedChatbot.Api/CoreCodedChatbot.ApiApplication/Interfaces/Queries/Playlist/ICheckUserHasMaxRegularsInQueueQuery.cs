namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist
{
    public interface ICheckUserHasMaxRegularsInQueueQuery
    {
        bool UserHasMaxRegularsInQueue(string username);
    }
}
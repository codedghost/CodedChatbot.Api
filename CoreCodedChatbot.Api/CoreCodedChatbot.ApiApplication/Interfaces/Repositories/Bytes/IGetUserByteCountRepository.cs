namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes
{
    public interface IGetUserByteCountRepository
    {
        float Get(string username, int byteConversion);
    }
}
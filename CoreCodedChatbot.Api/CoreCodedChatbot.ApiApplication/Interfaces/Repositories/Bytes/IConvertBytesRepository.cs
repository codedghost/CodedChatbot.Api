namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes
{
    public interface IConvertBytesRepository
    {
        int Convert(string username, int tokensToConvert, int byteConversion);
    }
}
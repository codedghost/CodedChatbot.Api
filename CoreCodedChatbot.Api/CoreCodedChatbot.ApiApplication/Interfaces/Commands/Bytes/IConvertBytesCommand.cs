namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;

public interface IConvertBytesCommand
{
    int Convert(string username, int bytesToConvert);
}
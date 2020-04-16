namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IDownloadChartService
    {
        void Download(string downloadUrl, int directoryId);
    }
}
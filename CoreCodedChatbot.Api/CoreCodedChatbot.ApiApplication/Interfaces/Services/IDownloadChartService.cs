using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IDownloadChartService
{
    Task Download(string downloadUrl, int directoryId);
}
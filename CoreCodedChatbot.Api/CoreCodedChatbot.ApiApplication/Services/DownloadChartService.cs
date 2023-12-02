using System.Threading.Tasks;
using CodedChatbot.ServiceBusContract;
using CodedGhost.AzureServiceBus.Abstractions;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services;

public class DownloadChartService : IBaseService, IDownloadChartService
{
    private readonly ISecretService _secretService;
    private readonly ILogger<IDownloadChartService> _logger;

    public DownloadChartService(
        ISecretService secretService,
        ILogger<IDownloadChartService> logger
    )
    {
        _secretService = secretService;
        _logger = logger;
    }

    public async Task Download(string downloadUrl, int directoryId)
    {
        var sender = new CodedServiceBusSender<ChartDownload>(_secretService);

        await sender.SendMessageAsync(new ChartDownload
        {
            DownloadUrl = downloadUrl,
            DirectoryName = directoryId.ToString()
        }).ConfigureAwait(false);
    }
}
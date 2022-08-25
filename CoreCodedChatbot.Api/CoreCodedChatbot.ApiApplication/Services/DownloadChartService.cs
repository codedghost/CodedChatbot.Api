using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.Logging;
using My.JDownloader.Api;
using My.JDownloader.Api.Models.LinkgrabberV2.Request;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class DownloadChartService : IDownloadChartService
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

        public void Download(string downloadUrl, int directoryId)
        {
            if (string.Equals(downloadUrl, "ubisoft", StringComparison.InvariantCultureIgnoreCase)) return;
            var jdownloaderUsername = _secretService.GetSecret<string>("JDownloaderUsername");
            var jdownloaderPassword = _secretService.GetSecret<string>("JDownloaderPassword");
            var jdownloaderAppKey = _secretService.GetSecret<string>("JDownloaderAppKey");

            var jdownloader = new JDownloaderHandler(jdownloaderUsername, jdownloaderPassword, jdownloaderAppKey);

            var devices = jdownloader.GetDevices();

            var mainDevice = devices.FirstOrDefault();

            if (mainDevice == null)
            {
                _logger.LogError("JDownloader instance not running or not discoverable");
                return;
            }

            var handler = jdownloader.GetDeviceHandler(mainDevice, true);

            handler.LinkgrabberV2.AddLinks(new AddLinkRequest
            {
                DestinationFolder = directoryId.ToString(),
                AutoExtract = true,
                AutoStart = true,
                Links = downloadUrl
            });

            handler.DownloadController.Start();
        }
    }
}
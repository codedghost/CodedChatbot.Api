using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Queries.AzureDevOps
{
    public class RaiseBugQuery : IRaiseBugQuery
    {
        private readonly IAzureDevOpsService _azureDevOpsService;

        public RaiseBugQuery(
            IAzureDevOpsService azureDevOpsService
        )
        {
            _azureDevOpsService = azureDevOpsService;
        }

        public async Task<bool> Raise(string twitchUsername, DevOpsBug bugInfo)
        {
            var result = await _azureDevOpsService.RaiseBugInBacklog(twitchUsername, bugInfo);

            return result;
        }
    }
}
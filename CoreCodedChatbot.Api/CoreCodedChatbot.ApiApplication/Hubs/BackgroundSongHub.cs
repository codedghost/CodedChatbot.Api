using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CoreCodedChatbot.ApiApplication.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BackgroundSongHub : Hub
    {
        public async Task TriggerBackgroundSongCheck(string username)
        {
            await Clients.All.SendAsync("BackgroundSongCheck", username);
        }
    }
}
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("SongSearch/[action]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SongSearchController : Controller
    {
        private readonly ISolrService _solrService;

        public SongSearchController(ISolrService solrService)
        {
            _solrService = solrService;
        }

        public async Task<IActionResult> Search(string artist, string song)
        {
            var result = await _solrService.Search(artist, song);

            return new JsonResult(result);
        }
    }
}
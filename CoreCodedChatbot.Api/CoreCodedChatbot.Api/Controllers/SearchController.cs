using CoreCodedChatbot.Api.Commands;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Search;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("Search/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public IActionResult SaveSearchSynonymRequest([FromBody]SaveSearchSynonymRequest request)
        {
            if (_searchService.SaveSearchSynonymRequest(request))
            {
                return Ok();
            }

            return BadRequest("Something went wrong");
        }
    }
}
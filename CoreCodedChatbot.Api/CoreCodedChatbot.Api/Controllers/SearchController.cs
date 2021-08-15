using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Search;
using CoreCodedChatbot.ApiContract.ResponseModels.Search;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("Search/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly ISolrService _solrService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(
            ISearchService searchService,
            ISolrService solrService,
            ILogger<SearchController> logger
        )
        {
            _searchService = searchService;
            _solrService = solrService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult SaveSearchSynonymRequest([FromBody] SaveSearchSynonymRequest request)
        {
            if (_searchService.SaveSearchSynonymRequest(request))
            {
                return Ok();
            }

            return BadRequest("Something went wrong");
        }

        [HttpPost]
        public async Task<IActionResult> SongSearch([FromBody] SongSearchRequest request)
        {
            try
            {
                var searchResults = await _solrService.Search(request.SearchTerms);

                return new JsonResult(new SongSearchResponse
                {
                    SearchResults = searchResults
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in SongSearch");
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> FormattedSongSearch([FromBody] FormattedSongSearchRequest request)
        {
            try
            {
                var searchResults = await _solrService.Search(request.ArtistName, request.SongName);

                return new JsonResult(new SongSearchResponse
                {
                    SearchResults = searchResults
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in FormattedSongSearch");
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult DownloadToOneDrive([FromBody] DownloadToOneDriveRequest request)
        {
            try
            {
                _searchService.DownloadSongToOneDrive(request.SongId);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in DownloadToOneDrive");
                return BadRequest();
            }
        }
    }
}
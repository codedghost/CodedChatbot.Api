﻿using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Repositories.Search
{
    public interface IGetSongsFromSearchResultsRepository
    {
        List<BasicSongSearchResult> Get(List<SongSearch> searchResults);
    }
}
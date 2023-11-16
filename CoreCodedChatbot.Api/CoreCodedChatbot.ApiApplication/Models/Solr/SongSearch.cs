using SolrNet.Attributes;

namespace CoreCodedChatbot.ApiApplication.Models.Solr;

public class SongSearch
{
    [SolrUniqueKey("SongId")]
    public int SongId { get; set; }

    [SolrField("SongName")]
    public string SongName { get; set; }

    [SolrField("SongArtist")]
    public string SongArtist { get; set; }
}
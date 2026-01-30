using Infertus.Geo.Location.Contracts.Models;

namespace Infertus.Geo.Location.Cache.Models;

public class SearchCacheItem
{
    public string SearchQuery { get; set; }
    public PlaceDTO ResponseInfo { get; set; }

    public SearchCacheItem(string searchQuery, PlaceDTO responseInfo)
    {
        SearchQuery = searchQuery;
        ResponseInfo = responseInfo;
    }
}

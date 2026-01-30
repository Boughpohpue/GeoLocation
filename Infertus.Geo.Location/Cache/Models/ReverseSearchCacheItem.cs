using Infertus.Geo.Location.Contracts.Models;

namespace Infertus.Geo.Location.Cache.Models;

public class ReverseSearchCacheItem
{
    public CoordinatesDTO SearchLatLon { get; set; }
    public PlaceDTO ResponseInfo { get; set; }

    public ReverseSearchCacheItem(CoordinatesDTO searchLatLon, PlaceDTO responseInfo)
    {
        SearchLatLon = searchLatLon;
        ResponseInfo = responseInfo;
    }
}

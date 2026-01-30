using Infertus.Geo.Location.Contracts.Models;
using Infertus.Geo.Location.Contracts.Responses;

namespace Infertus.Geo.Location.Cache.Interfaces;

public interface ILocationSearchCache
{
    void Add(SearchResponse searchResponse);
    void Add(ReverseSearchResponse searchResponse);
    PlaceDTO? GetSearchResult(string query);
    PlaceDTO? GetReverseSearchResult(CoordinatesDTO coordinates);
    PlaceDTO? GetReverseSearchResult(double latitude, double longitude);
}

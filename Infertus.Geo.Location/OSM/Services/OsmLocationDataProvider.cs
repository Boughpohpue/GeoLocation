using Infertus.Geo.Location.Contracts.Interfaces;
using Infertus.Geo.Location.Contracts.Models;
using Infertus.Geo.Location.Contracts.Requests;
using Infertus.Geo.Location.Contracts.Responses;
using Infertus.Geo.Location.OSM.Interfaces;
using Infertus.Mapper;

namespace Infertus.Geo.Location.OSM.Services;


public class OsmLocationDataProvider : ILocationDataProvider
{
    private readonly IOsmNominatimService _nominatimService;


    public OsmLocationDataProvider(IOsmNominatimService nominatimService)
    {
        _nominatimService = nominatimService;
    }


    public async Task<SearchResponse> Search(SearchRequest request)
    {
        var result = await _nominatimService.Search(request.AddressQuery).ConfigureAwait(false);

        return result == null
            ? new SearchResponse(request, null)
            : new SearchResponse(request, RMapper.Map<PlaceDTO>(result));
    }

    public async Task<ReverseSearchResponse> ReverseSearch(ReverseSearchRequest request)
    {
        var result = await _nominatimService.Reverse(
            request.Coordinates.Latitude, 
            request.Coordinates.Longitude)
        .ConfigureAwait(false);

        return result == null
            ? new ReverseSearchResponse(request, null)
            : new ReverseSearchResponse(request, RMapper.Map<PlaceDTO>(result));
    }
}

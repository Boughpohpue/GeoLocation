using Infertus.Geo.Location.Cache.Interfaces;
using Infertus.Geo.Location.Contracts.Interfaces;
using Infertus.Geo.Location.Contracts.Models;
using Infertus.Geo.Location.Contracts.Requests;
using Infertus.Geo.Location.Device.Interfaces;
using Infertus.Geo.Primitives;
using Infertus.Mapper;

namespace Infertus.Geo.Location;

public class LocationService : ILocationService
{
    private readonly ILocationSearchCache _searchCache;
    private readonly ILocationDataProvider _osmDataProvider;
    private readonly IDeviceLocationProvider _deviceDataProvider;


    public LocationService(
        ILocationSearchCache searchCache,
        ILocationDataProvider osmDataProvider,
        IDeviceLocationProvider deviceDataProvider)
    {
        _searchCache = searchCache;
        _osmDataProvider = osmDataProvider;
        _deviceDataProvider = deviceDataProvider;
    }

    public async Task<Coordinates?> GetDeviceCoordinates()
    {
        var coordinates = await _deviceDataProvider.GetCoordinates().ConfigureAwait(false);
        if (coordinates == null)
            return null;

        return RMapper.Map<Coordinates>(coordinates);
    }

    public async Task<Place?> GetPlaceyByDeviceLocation()
    {
        var location = await GetDeviceCoordinates().ConfigureAwait(false);
        if (location == null)
            return null;

        return await GetPlaceByLocation(location.Lat, location.Lon);
    }

    public async Task<Place?> GetPlaceByAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address) || address.Length <= 3)
            throw new ArgumentException($"Provided query must contain more than 3 characters!");

        var request = new SearchRequest(address);

        var repoResult = _searchCache.GetSearchResult(address);
        if (repoResult.HasValue)
            return RMapper.Map<Place>(repoResult.Value);

        var osmResult = await _osmDataProvider.Search(request).ConfigureAwait(false);
        if (osmResult.Place == null)
            return null;

        _searchCache.Add(osmResult);

        return RMapper.Map<Place>(osmResult.Place);
    }

    public async Task<Place?> GetPlaceByLocation(double latitude, double longitude)
    {
        var coord = new CoordinatesDTO(latitude, longitude);
        var request = new ReverseSearchRequest(coord);

        var repoResult = _searchCache.GetReverseSearchResult(request.Coordinates);
        if (repoResult != null)
            return RMapper.Map<Place>(repoResult.Value);

        var osmResult = await _osmDataProvider.ReverseSearch(request).ConfigureAwait(false);
        if (osmResult.Place == null)
            return null;

        _searchCache.Add(osmResult);

        return RMapper.Map<Place>(osmResult.Place);
    }
}

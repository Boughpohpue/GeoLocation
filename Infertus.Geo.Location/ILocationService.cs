using Infertus.Geo.Location.Contracts.Models;
using Infertus.Geo.Primitives;

namespace Infertus.Geo.Location;

public interface ILocationService
{
    Task<Coordinates?> GetDeviceCoordinates();
    Task<Place?> GetPlaceyByDeviceLocation();
    Task<Place?> GetPlaceByAddress(string address);
    Task<Place?> GetPlaceByLocation(double latitude, double longitute);
}

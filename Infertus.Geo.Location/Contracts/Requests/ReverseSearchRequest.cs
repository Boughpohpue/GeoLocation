using Infertus.Geo.Location.Contracts.Models;

namespace Infertus.Geo.Location.Contracts.Requests;

public readonly record struct ReverseSearchRequest(CoordinatesDTO Coordinates)
{
    public ReverseSearchRequest(double lat, double lon) : this(new CoordinatesDTO(lat, lon)) { }
}

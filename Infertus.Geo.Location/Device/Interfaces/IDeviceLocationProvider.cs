using Infertus.Geo.Location.Contracts.Models;

namespace Infertus.Geo.Location.Device.Interfaces;

public interface IDeviceLocationProvider
{
    Task<CoordinatesDTO?> GetCoordinates();
}

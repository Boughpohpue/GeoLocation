using Infertus.Geo.Location.Contracts.Models;
using System.Device.Location;

namespace Infertus.Geo.Location.Device.Interfaces;

public interface IDeviceLocationTracker
{
    event EventHandler<CoordinatesDTO>? LocationChanged;
    event EventHandler<GeoPositionStatus>? StatusChanged;

    void StartTracking();
    Task StartTrackingAsync();
    void StopTracking();
}

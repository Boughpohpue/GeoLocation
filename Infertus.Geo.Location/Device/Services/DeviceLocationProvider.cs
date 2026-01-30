using Infertus.Geo.Location.Contracts.Models;
using Infertus.Geo.Location.Device.Interfaces;
using Infertus.Mapper;
using System.Device.Location;

namespace Infertus.Geo.Location.Device.Services;

public class DeviceLocationProvider : IDeviceLocationProvider
{
    private const int SleepFor = 144;
    private const int TimeoutMs = 9696;

    private readonly GeoCoordinateWatcher _watcher;


    public DeviceLocationProvider()
    {
        _watcher = new(GeoPositionAccuracy.High);
    }

    public async Task<CoordinatesDTO?> GetCoordinates()
    {
        _watcher.Start();

        int waited = 0;
        while (_watcher.Status != GeoPositionStatus.Ready &&
               _watcher.Status != GeoPositionStatus.Disabled &&
               waited < TimeoutMs)
        {
            waited += SleepFor;
            Thread.Sleep(SleepFor);
        }

        CoordinatesDTO? result = null;
        if (_watcher.Status == GeoPositionStatus.Ready && !_watcher.Position.Location.IsUnknown)
            result = RMapper.Map<CoordinatesDTO>(_watcher.Position.Location);

        _watcher.Stop();

        return result;
    }
}

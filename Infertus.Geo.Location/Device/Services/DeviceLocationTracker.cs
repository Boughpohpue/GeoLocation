using Infertus.Geo.Location.Contracts.Models;
using Infertus.Geo.Location.Device.Interfaces;
using Infertus.Mapper;
using System.Device.Location;

namespace Infertus.Geo.Location.Device.Services;

public class DeviceLocationTracker : IDeviceLocationTracker
{
    private readonly GeoCoordinateWatcher _watcher;

    private bool _isRunning;
    private CancellationTokenSource? _cancellationTokenSource;

    public event EventHandler<CoordinatesDTO>? LocationChanged;
    public event EventHandler<GeoPositionStatus>? StatusChanged;


    public DeviceLocationTracker()
    {
        _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
    }


    public void StartTracking()
    {
        if (_isRunning)
            return;

        _watcher.StatusChanged += OnStatusChanged;
        _watcher.PositionChanged += OnPositionChanged;
        _watcher.Start();
        _isRunning = true;
    }

    public async Task StartTrackingAsync()
    {
        if (_isRunning)
            return;

        _cancellationTokenSource = new CancellationTokenSource();
        _watcher.StatusChanged += OnStatusChanged;
        _watcher.PositionChanged += OnPositionChanged;
        _watcher.Start();
        _isRunning = true;

        await Task.Delay(Timeout.Infinite, _cancellationTokenSource.Token);
    }

    public void StopTracking()
    {
        if (!_isRunning)
            return;

        try
        {
            _cancellationTokenSource?.Cancel();
            _isRunning = false;
            _watcher.Stop();

            _watcher.StatusChanged -= OnStatusChanged;
            _watcher.PositionChanged -= OnPositionChanged;
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Location tracking has been stopped.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while stopping device location tracking: {ex.Message}");
        }
    }

    private void OnStatusChanged(object? sender, GeoPositionStatusChangedEventArgs e)
    {
        StatusChanged?.Invoke(this, e.Status);
    }

    private void OnPositionChanged(object? sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
    {
        if (_watcher.Status == GeoPositionStatus.Ready && !e.Position.Location.IsUnknown)
            LocationChanged?.Invoke(this, RMapper.Map<CoordinatesDTO>(e.Position.Location));
    }
}

using Infertus.Geo.Location.Cache.Interfaces;
using Infertus.Geo.Location.Cache.Services;
using Infertus.Geo.Location.Contracts.Interfaces;
using Infertus.Geo.Location.Device.Interfaces;
using Infertus.Geo.Location.Device.Services;
using Infertus.Geo.Location.OSM.Interfaces;
using Infertus.Geo.Location.OSM.Services;
using Infertus.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Infertus.Geo.Location;

public static class InfertusGeoLocationServiceCollectionExtension
{
    public static IServiceCollection AddInfertusGeoLocation(this IServiceCollection services, string userAgent, string referer)
    {
        services.AddMappingProfile<MappingProfile>();
        services.AddHttpClient<IOsmNominatimService, OsmNominatimService>(client =>
        {
            client.DefaultRequestHeaders.Referrer = new Uri(referer);
            client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        });
        services.AddSingleton<ILocationDataProvider, OsmLocationDataProvider>();
        services.AddSingleton<IDeviceLocationProvider, DeviceLocationProvider>();
        services.AddSingleton<IDeviceLocationTracker, DeviceLocationTracker>();
        services.AddSingleton<ILocationSearchCache, LocationSearchCache>();
        services.AddSingleton<ILocationService, LocationService>();
        return services;
    }
}

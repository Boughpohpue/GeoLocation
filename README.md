# Infertus.Geo.Location

Infertus.Geo.Location is a lightweight .NET library designed to fetch geographic location information through various methods, including:

1. **Device GPS** – Retrieves coordinates from the device if GPS is available with live tracking functionality.
2. **OSM (OpenStreetMap) API** – Fetches coordinates and place information from OpenStreetMap by address queries or coordinates.
3. **Caching** – Caches previous queries to limit API calls for repeated searches.

This library is ideal for applications that need to determine location information without relying on a particular platform (e.g., Google Maps), and it supports both real-time GPS data and external location lookups.


## Features

* **OSM Integration** – Query location information using the OpenStreetMap Nominatim API.
* **Device Location** – Retrieve coordinates from the device’s GPS if available.
* **Live Location Tracking** - Continuously retrieve and update device coordinates in real-time using event-based notifications, making it ideal for apps requiring live tracking (e.g., navigation or fleet management).
* **Caching** – Cache address queries to limit redundant API requests and improve performance.


## Usage

Here's a sample usage of the library in a console application, demonstrating how to use the **device location**, **OSM search**, and **caching**.

### 1. Setup the `Program.cs`

```csharp
using Infertus.Geo.Location;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

const string AppId = "YourAppNameHere/1.0";
const string Referer = "https://your.referer.url.here";

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// Add necessary services for OSM, device, and base
builder.Services.AddInfertusGeoLocation(AppId, Referer);
```

### 2. Example Service Job

This example shows how to retrieve coordinates and place information, either from the device's GPS or from OpenStreetMap.

```csharp
static async Task RunServiceJob(IServiceProvider hostProvider)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    try
    {
        var locationSrv = provider.GetService<ILocationService>();
 
        Console.WriteLine("TRYING TO GET DEVICE LOCATION...");
        var devCoord = await locationSrv.GetDeviceCoordinates();
        if (devCoord != null)
        {
            Console.WriteLine($"DEVICE LOCATION DETECTED!\n{devCoord}");
            Console.WriteLine("TRYING TO GET PLACE INFO...");
            var place = await locationSrv.GetPlaceByLatLon(devCoord.Lat, devCoord.Lon);
            if (place.Response == null)
                Console.WriteLine("FAILED!\n");
            else
            {
                Console.WriteLine($"PLACE INFO FOUND!\n{place.Response}\n");
                Console.WriteLine("TRYING TO GET COORDINATES USING PLACE INFO...");
                var place2coord = await locationSrv.GetCoordinatesByAddress(
                    place.Response.Value.CoordinatesWithDisplayName.DisplayName);
                if (place2coord.Response == null)
                    Console.WriteLine("FAILED!\n");
                else
                    Console.WriteLine($"COORDINATES FOUND!\n{place2coord.Response}\n");
            }
        }
        else
        {
            Console.WriteLine("DEVICE LOCATION UNAVAILABLE!\n");
            Console.WriteLine("TESTING WITH EXAMPLE VALUES...");

            Console.WriteLine("TRYING TO GET PLACE BY COORDINATES...");
            var coord2place = await locationSrv.GetPlaceByLatLon(36.96369, 12.369144);
            if (coord2place.Response == null)
                Console.WriteLine("FAILED!\n");
            else
                Console.WriteLine($"SUCCESS!\n{coord2place.Response}\n");

            Console.WriteLine("TRYING TO GET COORDINATES BY PLACE...");
            var place2coord = await locationSrv.GetCoordinatesByAddress("Berlin Germany");
            if (place2coord.Response == null)
                Console.WriteLine("FAILED!\n");
            else
                Console.WriteLine($"SUCCESS!\n{place2coord.Response}\n");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("\nERROR OCCURED!");
        Console.WriteLine(ex.ToString());
    }
}
```


### 3. Services Overview

* **Device Location**: Uses GPS data to fetch device coordinates (if available). Continuous event-based live tracking.
* **OSM Location**: Queries OpenStreetMap's Nominatim API by address, and can reverse lookup using coordinates.
* **Caching**: Caches results for address lookups to prevent redundant API calls, improving performance.


### 4. Advanced Usage

You can integrate this library in a backend service, an ASP.NET Core application, or a more complex system, as it provides easy extensibility via dependency injection.


### 5. Dependency Injection

In `Program.cs`, ensure that you've added the necessary services using these methods:

```csharp
builder.Services.AddInfertusGeoLocationOsm(client =>
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd("YourApp/1.0");
    client.DefaultRequestHeaders.Referrer = new Uri("https://your-referer-url");
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
});

builder.Services.AddInfertusGeoLocationDevice();
builder.Services.AddInfertusGeoLocationBase();
```


### 6. Dependencies and Configuration

**System.Device.dll for Device GPS**

For device location functionality (i.e., GPS data), this library relies on the System.Device assembly, which is part of the .NET Framework and Windows SDK. It is not available via NuGet for .NET Core or later versions, but can be manually referenced.

If you're working with a .NET Framework project, you can easily add the reference to System.Device from the Windows directory:

Right-click on the References node in your Visual Studio project.

Select Add Reference...

Use 'Browse...' button to find and include the System.Device.dll assembly file.

It should be placed in %WINDIR% under same or similar path as below:

```xml
<Reference Include="System.Device">
  <HintPath>C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Device.dll</HintPath>
</Reference>
```

This reference allows you to access the device's GPS information, which is critical for fetching real-time location data.


## Configuration

To ensure smooth operation, consider the following:

* **OSM API**: Be mindful of query limits and usage restrictions of the OpenStreetMap API (Nominatim). Implement rate-limiting or use caching to reduce calls.
* **GPS Access**: Ensure that your application has appropriate permissions for accessing GPS data, especially when targeting mobile devices or environments like Windows or Linux.


## Contributing

Feel free to fork this repository, submit issues, or create pull requests. Contributions are always welcome!


## License

This project is licensed under the GPL-3.0 – see the [LICENSE](LICENSE) file for details.

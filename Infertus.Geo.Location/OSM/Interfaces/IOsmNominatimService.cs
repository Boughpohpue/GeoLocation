using Infertus.Geo.Location.OSM.Models;

namespace Infertus.Geo.Location.OSM.Interfaces;

public interface IOsmNominatimService
{
    Task<OsmSearchResponseEntry?> Search(string address);
    Task<OsmReverseResponseEntry?> Reverse(double latitude, double longitute);
}

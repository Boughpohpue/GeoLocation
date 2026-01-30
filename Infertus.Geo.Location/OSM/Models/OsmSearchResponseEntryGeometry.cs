using Newtonsoft.Json;

namespace Infertus.Geo.Location.OSM.Models;

public class OsmSearchResponseEntryGeometry
{
    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("coordinates")]
    public List<List<List<double>>> Coordinates { get; set; } = default!;
}

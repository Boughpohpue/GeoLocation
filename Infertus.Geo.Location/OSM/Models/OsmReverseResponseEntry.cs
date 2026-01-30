using Newtonsoft.Json;

namespace Infertus.Geo.Location.OSM.Models;


public class OsmReverseResponseEntry
{
    [JsonProperty("place_id")]
    public long PlaceId { get; set; }

    [JsonProperty("licence")]
    public string Licence { get; set; } = default!;

    [JsonProperty("osm_type")]
    public string OsmType { get; set; } = default!;

    [JsonProperty("osm_id")]
    public long OsmId { get; set; } = default!;

    [JsonProperty("lat")]
    public string Latitude { get; set; } = default!;

    [JsonProperty("lon")]
    public string Longitude { get; set; } = default!;

    [JsonProperty("category")]
    public string Category { get; set; } = default!;

    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("place_rank")]
    public long PlaceRank { get; set; } = default!;

    [JsonProperty("importance")]
    public double Importance { get; set; } = default!;

    [JsonProperty("addresstype")]
    public string AddressType { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("display_name")]
    public string DisplayName { get; set; } = default!;

    [JsonProperty("address")]
    public OsmReverseResponseEntryAddress Address { get; set; } = default!;

    [JsonProperty("boundingbox")]
    public List<string> BoundingBox { get; set; } = default!;
}

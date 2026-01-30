using Newtonsoft.Json;

namespace Infertus.Geo.Location.OSM.Models;

public class OsmReverseResponseEntryAddress
{
    [JsonProperty("house_number")]
    public string HouseNumber { get; set; } = default!;

    [JsonProperty("road")]
    public string Road { get; set; } = default!;

    [JsonProperty("neighbourhood")]
    public string Neighbourhood { get; set; } = default!;

    [JsonProperty("suburb")]
    public string Suburb { get; set; } = default!;

    [JsonProperty("borough")]
    public string Borough { get; set; } = default!;

    [JsonProperty("city")]
    public string City { get; set; } = default!;

    [JsonProperty("village")]
    public string Village { get; set; } = default!;

    [JsonProperty("municipality")]
    public string Municipality { get; set; } = default!;

    [JsonProperty("county")]
    public string County { get; set; } = default!;

    [JsonProperty("state")]
    public string State { get; set; } = default!;

    [JsonProperty("postcode")]
    public string Postcode { get; set; } = default!;

    [JsonProperty("country")]
    public string Country { get; set; } = default!;

    [JsonProperty("country_code")]
    public string CountryCode { get; set; } = default!;

    [JsonProperty("ISO3166-2-lvl4")]
    public string ISO { get; set; } = default!;
}

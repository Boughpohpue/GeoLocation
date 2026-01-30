using Infertus.Geo.Location.OSM.Interfaces;
using Infertus.Geo.Location.OSM.Models;
using Newtonsoft.Json;
using System.Net;

namespace Infertus.Geo.Location.OSM.Services;

public class OsmNominatimService : IOsmNominatimService
{
    private const string ServiceUrl = "http://nominatim.openstreetmap.org/";
    private const string SearchEndpoint = "search";
    private const string ReverseEndpoint = "reverse";

    private readonly HttpClient _client;
    private static readonly SemaphoreSlim _throttle = new(1, 1);


    public OsmNominatimService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri(ServiceUrl);
    }


    public async Task<OsmSearchResponseEntry?> Search(string address)
    {
        var result = await GetSearchResult(address).ConfigureAwait(false);

        return JsonConvert.DeserializeObject<List<OsmSearchResponseEntry>>(result)?.FirstOrDefault();
    }

    public async Task<OsmReverseResponseEntry?> Reverse(double latitude, double longitute)
    {
        var result = await GetReverseResult(latitude, longitute).ConfigureAwait(false);

        return JsonConvert.DeserializeObject<OsmReverseResponseEntry>(result);
    }


    private async Task<string> GetSearchResult(string address)
        => await GetApiResultAsync(GetSearchQueryString(address)).ConfigureAwait(false);

    private async Task<string> GetReverseResult(double latitude, double longitute)
        => await GetApiResultAsync(GetReverseQueryString(latitude, longitute)).ConfigureAwait(false);

    private async Task<string> GetApiResultAsync(string query)
    {
        await _throttle.WaitAsync().ConfigureAwait(false);
        try
        {
            var response = await SendWithRetryAsync(() =>
                _client.GetAsync(query)).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        finally
        {
            await Task.Delay(1000).ConfigureAwait(false);
            _throttle.Release();
        }
    }

    private async Task<HttpResponseMessage> SendWithRetryAsync(Func<Task<HttpResponseMessage>> send)
    {
        var response = await send().ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
            return response;

        if (response.StatusCode == HttpStatusCode.Forbidden)
            return response;

        if (response.StatusCode is not (HttpStatusCode.TooManyRequests
                                       or HttpStatusCode.ServiceUnavailable))
            return response;

        var delay = GetRetryAfter(response);

        if (delay is null || delay <= TimeSpan.Zero)
            return response;

        await Task.Delay(delay.Value).ConfigureAwait(false);

        return await send().ConfigureAwait(false);
    }

    private static TimeSpan? GetRetryAfter(HttpResponseMessage response)
    {
        if (response.Headers.RetryAfter == null)
            return null;

        if (response.Headers.RetryAfter.Delta.HasValue)
            return response.Headers.RetryAfter.Delta.Value;

        if (response.Headers.RetryAfter.Date.HasValue)
            return response.Headers.RetryAfter.Date.Value - DateTimeOffset.UtcNow;

        return null;
    }

    private string FixUrl(string query)
    {
        while (query.Contains("  ")) query = query.Replace("  ", " ");
        return Uri.EscapeDataString(query.ToLower()).Replace("%20", "+");
    }

    private string GetSearchQueryString(string query)
        => $"{SearchEndpoint}?q={FixUrl(query)}&addressdetails=1&format=jsonv2";

    private string GetReverseQueryString(double lat, double lon)
        => $"{ReverseEndpoint}?lat={lat}&lon={lon}&format=jsonv2";
}

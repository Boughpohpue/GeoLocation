using Infertus.Geo.Location.Cache.Interfaces;
using Infertus.Geo.Location.Cache.Models;
using Infertus.Geo.Location.Contracts.Models;
using Infertus.Geo.Location.Contracts.Responses;
using Infertus.Mapper;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Infertus.Geo.Location.Cache.Services
{

    public class LocationSearchCache : ILocationSearchCache
    {
        private const string SearchDataFileName = "osm_search.dat";
        private const string ReverseSearchDataFileName = "osm_reverse.dat";

        private readonly List<SearchCacheItem> _searchData;
        private readonly List<ReverseSearchCacheItem> _reverseSearchData;


        public LocationSearchCache()
        {
            _searchData = LoadSearchDataFromFile();
            _reverseSearchData = LoadReverseSearchDataFromFile();
        }


        public void Add(SearchResponse searchResponse)
        {
            var query = NormalizeQuery(searchResponse.Request.AddressQuery);
            if (searchResponse.Place == null || _searchData.Any(d => d.SearchQuery == query))
                return;

            _searchData.Add(new SearchCacheItem(query, searchResponse.Place.Value));

            SaveSearchDataToFile();
        }

        public void Add(ReverseSearchResponse searchResponse)
        {
            if (searchResponse.Place == null || _reverseSearchData.Any(x =>
                    x.SearchLatLon.Latitude == searchResponse.Request.Coordinates.Latitude
                    && x.SearchLatLon.Longitude == searchResponse.Request.Coordinates.Longitude))
                return;

            _reverseSearchData.Add(new ReverseSearchCacheItem(
                searchResponse.Request.Coordinates,
                RMapper.Map<PlaceDTO>(searchResponse.Place)));

            SaveReverseSearchDataToFile();
        }

        public PlaceDTO? GetSearchResult(string query)
        {
            return _searchData.FirstOrDefault(x =>
                x.SearchQuery == NormalizeQuery(query))?.ResponseInfo;
        }

        public PlaceDTO? GetReverseSearchResult(CoordinatesDTO coordinates)
        {
            return _reverseSearchData.FirstOrDefault(x => x.SearchLatLon.Latitude == coordinates.Latitude
                && x.SearchLatLon.Longitude == coordinates.Longitude)?.ResponseInfo;
        }

        public PlaceDTO? GetReverseSearchResult(double latitude, double longitude)
            => GetReverseSearchResult(new CoordinatesDTO(latitude, longitude));


        private string NormalizeQuery(string query)
        {
            var clean = query?.Trim().ToLowerInvariant() ?? "";
            clean = Regex.Replace(clean, @"\s+", " ");
            clean = Regex.Replace(clean, @"[^a-z0-9\s]", "");

            var tokens = clean.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(tokens, StringComparer.InvariantCulture);
            return string.Join(" ", tokens);
        }

        private List<SearchCacheItem> LoadSearchDataFromFile()
        {
            try
            {
                return JsonConvert.DeserializeObject<List<SearchCacheItem>>(File.ReadAllText(SearchDataFileName)) ?? [];
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return [];
            }
        }
        private List<ReverseSearchCacheItem> LoadReverseSearchDataFromFile()
        {
            try
            {
                return JsonConvert.DeserializeObject<List<ReverseSearchCacheItem>>(File.ReadAllText(ReverseSearchDataFileName)) ?? [];
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return [];
            }
        }
        private void SaveSearchDataToFile()
        {
            File.WriteAllText(SearchDataFileName, JsonConvert.SerializeObject(_searchData));
        }
        private void SaveReverseSearchDataToFile()
        {
            File.WriteAllText(ReverseSearchDataFileName, JsonConvert.SerializeObject(_reverseSearchData));
        }
    }

}

using Infertus.Geo.Location.Contracts.Requests;
using Infertus.Geo.Location.Contracts.Responses;

namespace Infertus.Geo.Location.Contracts.Interfaces;

public interface ILocationDataProvider
{
    Task<SearchResponse> Search(SearchRequest request);
    Task<ReverseSearchResponse> ReverseSearch(ReverseSearchRequest request);
}

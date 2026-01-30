using Infertus.Geo.Location.Contracts.Models;
using Infertus.Geo.Location.Contracts.Requests;

namespace Infertus.Geo.Location.Contracts.Responses;

public readonly record struct ReverseSearchResponse(ReverseSearchRequest Request, PlaceDTO? Place);

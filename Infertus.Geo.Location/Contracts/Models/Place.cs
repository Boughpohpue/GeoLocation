using Infertus.Geo.Primitives;

namespace Infertus.Geo.Location.Contracts.Models;

public readonly record struct Place(string Name, Coordinates Coordinates);

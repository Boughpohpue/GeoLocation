using Infertus.Geo.Location.Contracts.Models;
using Infertus.Geo.Location.OSM.Models;
using Infertus.Geo.Primitives;
using Infertus.Mapper;
using System.Device.Location;
using System.Globalization;

namespace Infertus.Geo.Location;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        RMapper.Register<GeoCoordinate, Coordinates>(gc =>
            new Coordinates(gc.Latitude, gc.Longitude));

        RMapper.Register<GeoCoordinate, CoordinatesDTO>(gc =>
            new CoordinatesDTO(gc.Latitude, gc.Longitude));

        RMapper.Register<Coordinates, CoordinatesDTO>(c =>
            new CoordinatesDTO(c.Lat, c.Lon));

        RMapper.Register<CoordinatesDTO, Coordinates>(dto =>
            new Coordinates(dto.Latitude, dto.Longitude));


        RMapper.Register<Place, PlaceDTO>(p =>
            new PlaceDTO(p.Name, RMapper.Map<CoordinatesDTO>(p.Coordinates)));

        RMapper.Register<PlaceDTO, Place>(p =>
            new Place(p.Name, RMapper.Map<Coordinates>(p.Coordinates)));


        RMapper.Register<OsmSearchResponseEntry, PlaceDTO>(e =>
            new PlaceDTO(
                e.DisplayName, 
                new CoordinatesDTO(
                    double.Parse(e.Latitude, CultureInfo.InvariantCulture),
                    double.Parse(e.Longitude, CultureInfo.InvariantCulture))));

        RMapper.Register<OsmReverseResponseEntry, PlaceDTO>(e =>
            new PlaceDTO(
                e.DisplayName,
                new CoordinatesDTO(
                    double.Parse(e.Latitude, CultureInfo.InvariantCulture),
                    double.Parse(e.Longitude, CultureInfo.InvariantCulture))));
    }
}

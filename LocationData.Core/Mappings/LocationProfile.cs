namespace LocationData.Core.Mappings
{
    using AutoMapper;
    using Models;
    using Models.City;
    using Models.Weather;
    using System.Linq;

    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<City, CombinedLocationData>()
                .ForMember(m => m.Latitude, x => x.MapFrom( l => l.Latlng[1]))
                .ForMember(m => m.Longitude, x => x.MapFrom( l => l.Latlng[0]))
                .ForMember(m => m.Weather, x => x.Ignore())
                .ForMember(m => m.Currency, x => x.MapFrom(c => c.Currencies.FirstOrDefault()));
            CreateMap<WeatherData, CombinedLocationData>()
                .ForMember(s => s.Weather,
                    x => x.MapFrom(
                        w => w.Weather.FirstOrDefault().Description))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}

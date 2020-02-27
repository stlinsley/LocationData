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
                .ForMember(m => m.Weather, x => x.Ignore());
            CreateMap<WeatherData, CombinedLocationData>()
                .ForMember(s => s.Weather,
                    x => x.MapFrom(
                        w => w.Weather.FirstOrDefault().Description))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}

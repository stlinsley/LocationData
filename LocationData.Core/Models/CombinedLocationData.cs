namespace LocationData.Core.Models
{
    using System.Collections.Generic;
    using City;

    public class CombinedLocationData : ICombinedLocationData
    {
        public string CityName { get; set; }
        public string Country { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public string Population { get; set; }
        public List<long> Latlng { get; set; }
        public long Latitude { get => Latlng[1]; }
        public long Longitude { get => Latlng[0]; }
        public List<Currency> Currencies { get; set; }
        public string Weather { get; set; }
    }
}

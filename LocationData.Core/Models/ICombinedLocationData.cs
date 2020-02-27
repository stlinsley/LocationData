namespace LocationData.Core.Models
{
    using System.Collections.Generic;
    using City;

    public interface ICombinedLocationData
    {
        string Alpha2Code { get; set; }
        string Alpha3Code { get; set; }
        string CityName { get; set; }
        string Country { get; set; }
        List<Currency> Currencies { get; set; }
        List<long> Latlng { get; set; }
        long Latitude { get; }
        long Longitude { get; }
        string Population { get; set; }
        string Weather { get; set; }
    }
}
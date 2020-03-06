namespace LocationData.Core.Models
{
    using City;
    using System;

    public interface ICombinedLocationData
    {
        Guid Id { get; set; }
        string Alpha2Code { get; set; }
        string Alpha3Code { get; set; }
        string CityName { get; set; }
        string Country { get; set; }
        int CurrencyId { get; set; }
        Currency Currency { get; set; }
        decimal Latitude { get; }
        decimal Longitude { get; }
        long Population { get; set; }
        string Weather { get; set; }
        int TouristRating { get; set; }
    }
}
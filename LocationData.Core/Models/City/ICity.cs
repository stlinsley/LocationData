namespace LocationData.Core.Models.City
{
    using System.Collections.Generic;

    public interface ICity
    {
        string Country { get; set; }
        string Alpha2Code { get; set; }
        string Alpha3Code { get; set; }
        string CityName { get; set; }
        long Population { get; set; }
        List<decimal> Latlng { get; set; }
        List<Currency> Currencies { get; set; }
    }
}
namespace LocationData.Core.Models
{
    using City;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CombinedLocationData : ICombinedLocationData
    {
        public Guid Id { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public long Population { get; set; }
        [Column(TypeName = "decimal")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal")]
        public decimal Longitude { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public string Weather { get; set; }
        public int TouristRating { get; set; }
    }
}

namespace LocationData.Core.Models.City
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class City : ICity
    {
        [JsonProperty("name")]
        public string Country { get; set; }

        [JsonProperty("alpha2Code")]
        public string Alpha2Code { get; set; }

        [JsonProperty("alpha3Code")]
        public string Alpha3Code { get; set; }

        [JsonProperty("capital")]
        public string CityName { get; set; }

        [JsonProperty("population")]
        public long Population { get; set; }

        [JsonProperty("latlng")]
        public List<decimal> Latlng { get; set; }

        [JsonProperty("currencies")]
        public List<Currency> Currencies { get; set; }

    }
}

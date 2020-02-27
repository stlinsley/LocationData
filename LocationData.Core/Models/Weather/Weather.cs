namespace LocationData.Core.Models.Weather
{
    using Newtonsoft.Json;

    public class Weather : IWeather

    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
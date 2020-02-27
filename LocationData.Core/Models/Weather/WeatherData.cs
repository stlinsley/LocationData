namespace LocationData.Core.Models.Weather
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class WeatherData : IWeatherData
    {
        [JsonProperty("coord")] 
        public Coord Coord { get; set; }

        [JsonProperty("weather")] 
        public List<Weather> Weather { get; set; }
    }
}

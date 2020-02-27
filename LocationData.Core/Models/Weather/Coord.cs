namespace LocationData.Core.Models.Weather
{
    using Newtonsoft.Json;

    public class Coord : ICoord
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }
}
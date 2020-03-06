namespace LocationData.Core.Models.City
{
    using Newtonsoft.Json;

    public class Currency : ICurrency
    {
        public int CurrencyId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}
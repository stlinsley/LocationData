namespace LocationData.ApiRepository.Facades
{
    using Microsoft.Extensions.Options;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class WeatherApiFacade : IWeatherDataFacade
    {
        private readonly string _apiKey;
        private readonly string _baseUri;
        private readonly IClient _client;
        private readonly ISerialization _serialization;

        public WeatherApiFacade(IClient client, IOptionsMonitor<WeatherDataFacadeOptions> options, ISerialization serialization)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _serialization = serialization ?? throw new ArgumentNullException(nameof(serialization));
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _baseUri = options.CurrentValue.BaseUri;
            _apiKey = options.CurrentValue.ApiKey;
        }

        public async Task<T> GetWeatherDataForLngLat<T>(decimal lng, decimal lat)
        {
            var response = await _client.SendAsyncRequest(_client.CreateClient(), CreateRequest(lng, lat), HttpCompletionOption.ResponseHeadersRead);
            return await _serialization.Deserialize<T>(response);
        }

        private HttpRequestMessage CreateRequest(decimal lng, decimal lat)
        {
            return new HttpRequestMessage(HttpMethod.Get, $"{_baseUri}lat={lat}&lon={lng}&appid={_apiKey}");
        }
    }
}
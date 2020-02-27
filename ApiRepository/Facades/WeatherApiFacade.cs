namespace LocationData.ApiRepository.Facades
{
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class WeatherApiFacade : IWeatherDataFacade
    {
        private readonly string _apiKey;
        private readonly string _baseUri;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializer _serializer;

        public WeatherApiFacade(IHttpClientFactory httpClientFactory, IOptionsMonitor<WeatherDataFacadeOptions> options, JsonSerializer serializer)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _baseUri = options.CurrentValue.BaseUri;
            _apiKey = options.CurrentValue.ApiKey;
        }

        public async Task<T> GetWeatherDataForLngLat<T>(decimal lng, decimal lat)
        {
            var client = _httpClientFactory.CreateClient();

            var request = CreateRequest(lng, lat);

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            using (var textReader = new JsonTextReader(streamReader))
            {
                return await Task.Run(() => _serializer.Deserialize<T>(textReader));
            }
        }

        private HttpRequestMessage CreateRequest(decimal lng, decimal lat)
        {
            return new HttpRequestMessage(HttpMethod.Get, $"{_baseUri}lat={lat}&lon={lng}&appid={_apiKey}");
        }
    }
}
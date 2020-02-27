namespace LocationData.ApiRepository.Facades
{
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class CityApiFacade : ICityDataFacade
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializer _serializer;
        private readonly string _baseUri;

        public CityApiFacade(IHttpClientFactory httpClientFactory, IOptionsMonitor<CityDataFacadeOptions> options, JsonSerializer serializer)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _baseUri = options.CurrentValue.BaseUri;
        }

        public async Task<List<T>> GetCityData<T>(string city)
        {
            if (string.IsNullOrEmpty(city)) throw new ArgumentNullException(nameof(city));
            var client = _httpClientFactory.CreateClient();

            var request = CreateRequest(city);

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            using (var textReader = new JsonTextReader(streamReader))
            {
                return await Task.Run(() => _serializer.Deserialize<List<T>>(textReader));
            }
        }

        private HttpRequestMessage CreateRequest(string city)
        {
            return new HttpRequestMessage(HttpMethod.Get, $"{_baseUri}/capital/{city}");
        }
    }
}

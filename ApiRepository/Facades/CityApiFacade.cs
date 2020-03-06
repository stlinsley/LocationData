namespace LocationData.ApiRepository.Facades
{
    using Microsoft.Extensions.Options;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CityApiFacade : ICityDataFacade
    {
        private readonly IClient _client;
        private readonly ISerialization _serialization;
        private readonly string _baseUri;

        public CityApiFacade(IClient client, IOptionsMonitor<CityDataFacadeOptions> options, ISerialization serialization)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _serialization = serialization ?? throw new ArgumentNullException(nameof(serialization));
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _baseUri = options.CurrentValue.BaseUri;
        }

        public async Task<T> GetCityData<T>(string city)
        {
            if (string.IsNullOrEmpty(city)) throw new ArgumentNullException(nameof(city));
            var response = await _client.SendAsyncRequest(_client.CreateClient(), CreateRequest(city), HttpCompletionOption.ResponseHeadersRead);
            //return await _serialization.Deserialize<T>(response);
            return await _serialization.Deserialize<T>(response);
        }

        private HttpRequestMessage CreateRequest(string city)
        {
            if (city == null) throw new ArgumentNullException(nameof(city));
            return new HttpRequestMessage(HttpMethod.Get, $"{_baseUri}/capital/{city}");
        }
    }
}

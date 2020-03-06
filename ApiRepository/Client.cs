namespace LocationData.ApiRepository
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Client : IClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _client;

        public Client(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public HttpClient CreateClient()
        {
            _client = _httpClientFactory.CreateClient();
            return _client;
        }

        public async Task<HttpResponseMessage> SendAsyncRequest(HttpClient client, HttpRequestMessage request, HttpCompletionOption option)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (request == null) throw new ArgumentNullException(nameof(request));
            return await client.SendAsync(request, option).ConfigureAwait(false);
        }
    }
}

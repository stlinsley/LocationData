using System.Net.Http;
using System.Threading.Tasks;

namespace LocationData.ApiRepository
{
    public interface IClient
    {
        HttpClient CreateClient();
        Task<HttpResponseMessage> SendAsyncRequest(HttpClient client, HttpRequestMessage request, HttpCompletionOption option);
    }
}
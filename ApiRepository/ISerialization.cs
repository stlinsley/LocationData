using System.Net.Http;
using System.Threading.Tasks;

namespace LocationData.ApiRepository
{
    public interface ISerialization
    {
        Task<T> Deserialize<T>(HttpResponseMessage response);
    }
}
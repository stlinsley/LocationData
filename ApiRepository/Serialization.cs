namespace LocationData.ApiRepository
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Serialization : ISerialization
    {
        private readonly JsonSerializer _serializer;

        public Serialization(JsonSerializer serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async Task<T> Deserialize<T>(HttpResponseMessage response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}

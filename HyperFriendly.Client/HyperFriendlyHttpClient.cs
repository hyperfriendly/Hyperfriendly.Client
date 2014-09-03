using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HyperFriendly.Client
{
    public class HyperFriendlyHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _rootUri;

        public HyperFriendlyHttpClient(HttpClient httpClient, string rootUri)
        {
            _httpClient = httpClient;
            _rootUri = rootUri;
        }

        public async Task<HyperFriendlyHttpClient> Root()
        {
            var result = await _httpClient.GetAsync(_rootUri);
            return new HyperFriendlyHttpClient(_httpClient, _rootUri)
            {
                CurrentResult = result
            };
        }

        public HttpResponseMessage CurrentResult { get; private set; }

        public async Task<dynamic> ResultAsJson()
        {
            var content = await CurrentResult.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(content);
        }

        public async Task<HyperFriendlyHttpClient> Follow(string rel)
        {
            JToken json = await ResultAsJson();
            var links = json.SelectToken("_links." + rel);
            var href = links.Value<string>("href");

            var result = await _httpClient.GetAsync(href);
            return new HyperFriendlyHttpClient(_httpClient, _rootUri)
            {
                CurrentResult = result
            };
        }
    }
}
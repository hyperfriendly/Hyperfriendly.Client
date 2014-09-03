using System.Net.Http;
using System.Threading.Tasks;

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
    }
}
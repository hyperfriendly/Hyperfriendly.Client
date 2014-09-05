using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HyperFriendly.Client
{
    public class HyperFriendlyHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _rootUri;
        private readonly QueryStringComposer _queryStringComposer;

        public HttpResponseMessage CurrentResult { get; private set; }

        public HyperFriendlyHttpClient(HttpClient httpClient, string rootUri)
        {
            _httpClient = httpClient;
            _rootUri = rootUri;
            _queryStringComposer = new QueryStringComposer();
        }

        public async Task<HyperFriendlyHttpClient> Root()
        {
            var result = await _httpClient.GetAsync(_rootUri);
            return new HyperFriendlyHttpClient(_httpClient, _rootUri)
            {
                CurrentResult = result
            };
        }

        public async Task<dynamic> ResultAsJson()
        {
            var httpContent = CurrentResult.Content;
            var content = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(content);
        }

        public async Task<HyperFriendlyHttpClient> Follow(string rel)
        {
            var href = await GetHref(rel);

            var result = await _httpClient.GetAsync(href);
            return new HyperFriendlyHttpClient(_httpClient, _rootUri)
            {
                CurrentResult = result
            };
        }

        public async Task<HyperFriendlyHttpClient> Follow(string rel, object arguments)
        {
            var href = await GetHref(rel);
            var uri = _queryStringComposer.Compose(href, arguments);
            var result = await _httpClient.GetAsync(uri);
            return new HyperFriendlyHttpClient(_httpClient, _rootUri)
            {
                CurrentResult = result
            };
        }

        private async Task<string> GetHref(string rel)
        {
            JToken json = await ResultAsJson();
            var links = json.SelectToken("_links." + rel);
            return links.Value<string>("href");
        }
    }
}
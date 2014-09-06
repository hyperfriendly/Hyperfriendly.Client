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

        public async Task<dynamic> ResultAsJson()
        {
            var httpContent = CurrentResult.Content;
            var content = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(content);
        }

        public async Task<dynamic> Result<T>()
        {
            var httpContent = CurrentResult.Content;
            var content = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<HyperFriendlyHttpClient> Root()
        {
            var result = await _httpClient.GetAsync(_rootUri);
            return new HyperFriendlyHttpClient(_httpClient, _rootUri)
            {
                CurrentResult = result
            };
        }

        public async Task<HyperFriendlyHttpClient> Follow(string rel)
        {
            var link = await GetLink(rel);

            return await GetResult(link.Href, link.HttpMethod);
        }

        public async Task<HyperFriendlyHttpClient> Follow(string rel, object arguments)
        {
            var link = await GetLink(rel);
            var href = _queryStringComposer.Compose(link.Href, arguments);
            return await GetResult(href, link.HttpMethod);
        }

        private async Task<HyperFriendlyHttpClient> GetResult(string href, HttpMethod httpMethod)
        {
            var result = await _httpClient.SendAsync(new HttpRequestMessage(httpMethod, href));
            return new HyperFriendlyHttpClient(_httpClient, _rootUri)
            {
                CurrentResult = result
            };
        }

        public async Task<HyperFriendlyHttpClient> Follow()
        {
            var location = CurrentResult.Headers.Location;
            var result = await _httpClient.GetAsync(location.ToString());
            return new HyperFriendlyHttpClient(_httpClient, _rootUri)
            {
                CurrentResult = result
            };
        }

        private async Task<Link> GetLink(string rel)
        {
            JToken json = await ResultAsJson();
            var link = json.SelectToken("_links." + rel);
            return link == null ? null : link.ToObject<Link>();
        }

        public async Task<bool> CanFollow(string rel)
        {
            return await GetLink(rel) != null;
        }
    }
}
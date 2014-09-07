using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HyperFriendly.Client
{
    public class HyperFriendlyHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _rootUri;
        private readonly QueryStringComposer _queryStringComposer;
        private HttpResponseMessage _currentResult;

        public HttpResponseMessage CurrentResult
        {
            get { return _currentResult; }
            private set
            {
                if (_currentResult != null)
                    _currentResult.Dispose();
                _currentResult = value;
            }
        }

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

        public async Task<IEnumerable<T>> CollectionResult<T>()
        {
            var httpContent = CurrentResult.Content;
            var content = await httpContent.ReadAsStringAsync();
            var json = (JToken)JsonConvert.DeserializeObject(content);
            return json.SelectToken("_items").Select(t => t.ToObject<T>());
        }

        public async Task Root()
        {
            var result = await _httpClient.GetAsync(_rootUri);
            CurrentResult = result;
        }

        public async Task Follow(string rel, object arguments = null)
        {
            var link = await GetLink(rel);
            var href = arguments != null
                ? _queryStringComposer.Compose(link.Href, arguments)
                : link.Href;
            await GetResult(href, link.HttpMethod);
        }

        private async Task GetResult(string href, HttpMethod httpMethod)
        {
            var result = await _httpClient.SendAsync(new HttpRequestMessage(httpMethod, href));
            CurrentResult = result;
        }

        public async Task Follow()
        {
            var location = CurrentResult.Headers.Location;
            var result = await _httpClient.GetAsync(location.ToString());
            CurrentResult = result;
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

        public bool CanFollow()
        {
            return CurrentResult.Headers.Location != null;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _currentResult.Dispose();
        }
    }
}
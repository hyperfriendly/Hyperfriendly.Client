using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HyperFriendly.Client
{
    public class HyperFriendlyHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _rootUri;
        private readonly QueryStringComposer _queryStringComposer;

        public HyperFriendlyResult CurrentResult { get; private set; }

        public HyperFriendlyHttpClient(HttpClient httpClient, string rootUri)
        {
            _httpClient = httpClient;
            _rootUri = rootUri;
            _queryStringComposer = new QueryStringComposer();
        }

        public async Task RootAsync()
        {
            await GetResultAsync(_rootUri, HttpMethod.Get);
        }

        public async Task FollowAsync(string rel, object content = null, object arguments = null)
        {
            var link = CurrentResult.GetLink(rel);
            var href = arguments != null
                ? _queryStringComposer.Compose(link.Href, arguments)
                : link.Href;
            await GetResultAsync(href, link.HttpMethod, content);
        }

        public async Task FollowAsync()
        {
            var location = CurrentResult.Headers.Location.AbsoluteUri;
            await GetResultAsync(location, HttpMethod.Get);
        }

        private async Task GetResultAsync(string href, HttpMethod httpMethod, object content = null)
        {
            var message = new HttpRequestMessage(httpMethod, href);
            if (content != null)
                message.Content = new ObjectContent(content.GetType(), content, new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/json"));
            var result = await _httpClient.SendAsync(message);
            var jsonString = await result.Content.ReadAsStringAsync();
            CurrentResult = new HyperFriendlyResult(jsonString, result.Headers, result.StatusCode);
        }

        public bool CanFollow(string rel)
        {
            return CurrentResult.GetLink(rel) != null;
        }

        public bool CanFollow()
        {
            return CurrentResult.Headers.Location != null;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
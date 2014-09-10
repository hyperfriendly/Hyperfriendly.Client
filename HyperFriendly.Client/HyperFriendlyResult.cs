using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HyperFriendly.Client
{
    public class HyperFriendlyResult
    {
        public HttpStatusCode StatusCode { get; private set; }
        private readonly string _content;
        private readonly HttpResponseHeaders _headers;

        public HyperFriendlyResult(string content, HttpResponseHeaders headers, HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            _content = content;
            _headers = headers;
        }

        public HttpResponseHeaders Headers
        {
            get { return _headers; }
        }

        public dynamic ToJson()
        {
            return JsonConvert.DeserializeObject(_content);
        }

        public dynamic ToObject<T>()
        {
            return JsonConvert.DeserializeObject<T>(_content);
        }

        public IEnumerable<T> CollectionResult<T>()
        {
            var json = (JToken)JsonConvert.DeserializeObject(_content);
            return json.SelectToken("_items").Select(t => t.ToObject<T>());
        }

        public Link GetLink(string rel)
        {
            JToken json = ToJson();
            var link = json.SelectToken("_links." + rel);
            return link == null ? null : link.ToObject<Link>();
        }
    }
}
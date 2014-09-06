using System;
using System.Net;
using System.Net.Http;

namespace HyperFriendly.Client
{
    public class Link
    {
        private string _method;

        public Link(string href)
        {
            Href = href;
        }

        public string Href { get; private set; }

        public string Method
        {
            get { return _method ?? WebRequestMethods.Http.Get; }
            set { _method = value; }
        }

        public HttpMethod HttpMethod
        {
            get
            {
                switch (Method.ToUpper())
                {
                    case "GET":
                        return HttpMethod.Get;
                    case "POST":
                        return HttpMethod.Post;
                    case "PUT":
                        return HttpMethod.Put;
                    case "DELETE":
                        return HttpMethod.Delete;
                    default:
                        throw new ArgumentException(string.Format("Http method '{0}' is not supported", Method));
                }
            }
        }
    }
}
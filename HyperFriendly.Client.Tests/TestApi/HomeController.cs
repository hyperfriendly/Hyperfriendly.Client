using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HyperFriendly.Client.Tests.TestApi
{
    public class HomeController : ApiController
    {
        [Route("", Name = "Home")]
        public HttpResponseMessage Get()
        {
            var resource = new
            {
                _links = new
                {
                    self = new { href = Url.Link("Home", null) },
                    some_resource = new { href = Url.Link("SomeResource", null)}
                }
            };
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("someresource", Name = "SomeResource")]
        public HttpResponseMessage GetSomeResource()
        {
            var resource = new
            {
                _links = new
                {
                    self = new { href = Url.Link("SomeResource", null) }
                },
                type = "some_resource"
            };
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }
    }
}
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
                    self = new { href = "/" },
                    some_resource = new { href = "/someresource" },
                    templated_resource = new { href = "/templated?foo={foo}" },
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
                    self = new { href = "/someresource" }
                },
                type = "some_resource"
            };
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("templated", Name = "TemplatedResource")]
        public HttpResponseMessage GetTemplatedResource(string foo)
        {
            var resource = new
            {
                _links = new
                {
                    self = new { href = "/someresource?foo=" + foo }
                },
                type = "templated_resource"
            };
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }
    }
}
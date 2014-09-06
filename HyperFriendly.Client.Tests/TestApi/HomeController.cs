using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HyperFriendly.Client.Tests.TestApi
{
    public class HomeController : ApiController
    {
        [Route("")]
        public HttpResponseMessage Get()
        {
            var resource = new
            {
                _links = new
                {
                    self = new { href = "/" },
                    some_resource = new { href = "/someresource" },
                    templated_resource = new { href = "/templated?foo={foo}" },
                    redirecting_resource = new { href = "/redirecting_resource" },
                    post_resource = new { href = "/post_resource", method = "POST" },
                    put_resource = new { href = "/put_resource", method = "PUT" },
                    delete_resource = new { href = "/delete_resource", method = "DELETE" },
                }
            };
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("someresource")]
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

        [Route("post_resource")]
        public HttpResponseMessage Post()
        {
            var resource = CreateResource("post_resource");
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("put_resource")]
        public HttpResponseMessage Put()
        {
            var resource = CreateResource("put_resource");
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("delete_resource")]
        public HttpResponseMessage Delete()
        {
            var resource = CreateResource("delete_resource");
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        private static object CreateResource(string rel)
        {
            var resource = new
            {
                _links = new
                {
                    self = new { href = "/" + rel }
                },
                type = rel
            };
            return resource;
        }

        [Route("templated")]
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

        [Route("redirecting_resource")]
        public HttpResponseMessage GetRedirectingResource()
        {
            var response = Request.CreateResponse();

            response.Headers.Location = new Uri(Url.Link("RedirectedTo", null));
            return response;
        }

        [Route("resource_that_is_redirected_to", Name = "RedirectedTo")]
        public HttpResponseMessage GetResourceThatIsRedirectedTo()
        {
            var resource = new
            {
                _links = new
                {
                    self = new { href = "/resource_that_is_redirected_to" }
                },
                type = "resource_that_is_redirected_to"
            };
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }
    }
}
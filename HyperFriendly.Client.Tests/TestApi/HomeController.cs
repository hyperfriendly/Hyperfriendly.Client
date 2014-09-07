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
                    some_resource_with_content = new { href = "/some_resource_with_content", method = "POST" },
                    templated_resource_with_content = new { href = "/templated_resource_with_content", method = "POST" },
                    collection_resource = new { href = "/collection_resource" },
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
                type = "some_resource"
            };
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("templated_resource_with_content")]
        public HttpResponseMessage PostTemplatedResourceWithContent(dynamic content)
        {
            var resource = new
            {
                type = "templated_resource_with_content",
                content.foo
            };
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("some_resource_with_content")]
        public HttpResponseMessage PostSomeResourceWithContent(dynamic content)
        {
            var resource = new
            {
                type = "some_resource_with_content", content.foo
            };
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("collection_resource")]
        public HttpResponseMessage GetCollectionResource()
        {
            var resource = new
            {
                _items = new[]
                {
                    new { type = "some_resource"},
                    new { type = "some_resource_2"}
                }
            };
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("post_resource")]
        public HttpResponseMessage Post()
        {
            var resource1 = new
            {
                type = "post_resource"
            };
            var resource = (object) resource1;
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("put_resource")]
        public HttpResponseMessage Put()
        {
            var resource1 = new
            {
                type = "put_resource"
            };
            var resource = (object) resource1;
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("delete_resource")]
        public HttpResponseMessage Delete()
        {
            var resource1 = new
            {
                type = "delete_resource"
            };
            var resource = (object) resource1;
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }

        [Route("templated")]
        public HttpResponseMessage GetTemplatedResource(string foo)
        {
            var resource = new
            {
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
                type = "resource_that_is_redirected_to"
            };
            return Request.CreateResponse(HttpStatusCode.OK, resource, "vnd/hyperfriendly+json");
        }
    }
}
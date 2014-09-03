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
            return Request.CreateResponse(HttpStatusCode.OK, new {}, "vnd/hyperfriendly+json");
        }
    }
}
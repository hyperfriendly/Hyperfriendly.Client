using System.Threading.Tasks;
using HyperFriendly.Client.Tests.TestApi;
using Microsoft.Owin.Testing;
using Newtonsoft.Json.Linq;
using Should;
using Xunit;

namespace HyperFriendly.Client.Tests
{
    public class following_a_location_header
    {
        [Fact]
        public async Task can_get_a_resource_by_following_the_location_header_of_another_resource()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);

            await client.Root();
            await client.Follow("redirecting_resource");
            await client.Follow();
            JToken json = await client.ResultAsJson();

            json.Value<string>("type").ShouldEqual("resource_that_is_redirected_to");
        }
    }
}
using System.Threading.Tasks;
using HyperFriendly.Client.Tests.TestApi;
using Microsoft.Owin.Testing;
using Newtonsoft.Json.Linq;
using Should;
using Xunit;

namespace HyperFriendly.Client.Tests
{
    public class following_a_link
    {
        private const string HomeUri = "http://localhost:1337/";

        [Fact]
        public async Task can_get_a_resource_by_following_a_rel()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, HomeUri);

            var response = await client.Root();
            client = await response.Follow("some_resource");
            JToken json = await client.ResultAsJson();

            json.Value<string>("type").ShouldEqual("some_resource");
        }
    }
}
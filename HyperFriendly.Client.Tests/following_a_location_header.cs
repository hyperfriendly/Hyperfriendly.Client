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

        [Fact]
        public async Task can_follow_is_true_when_location_header_exists()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);
            await client.Root();
            await client.Follow("redirecting_resource");

            var canFollow = client.CanFollow();

            canFollow.ShouldBeTrue();
        }

        [Fact]
        public async Task can_follow_is_false_when_location_header_does_not_exist()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);
            await client.Root();
            await client.Follow("some_resource");

            var canFollow = client.CanFollow();

            canFollow.ShouldBeFalse();
        }
    }
}
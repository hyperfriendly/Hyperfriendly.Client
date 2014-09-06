using System.Threading.Tasks;
using HyperFriendly.Client.Tests.TestApi;
using Microsoft.Owin.Testing;
using Newtonsoft.Json.Linq;
using Should;
using Xunit;
using Xunit.Extensions;

namespace HyperFriendly.Client.Tests
{
    public class following_a_link
    {
        [Fact]
        public async Task can_get_a_resource_by_following_a_rel()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);

            client = await client.Root();
            client = await client.Follow("some_resource");
            JToken json = await client.ResultAsJson();

            json.Value<string>("type").ShouldEqual("some_resource");
        }

        [Fact]
        public async Task can_get_a_resource_content_as_a_serialized_object()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);

            client = await client.Root();
            client = await client.Follow("some_resource");
            SomeResource resource = await client.Result<SomeResource>();

            resource.Type.ShouldEqual("some_resource");
        }

        [Fact]
        public async Task can_follow_is_true_when_rel_exists()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);
            client = await client.Root();

            var canFollow = await client.CanFollow("some_resource");

            canFollow.ShouldBeTrue();
        }

        [Fact]
        public async Task can_follow_is_true_when_rel_does_not_exist()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);
            client = await client.Root();

            var canFollow = await client.CanFollow("not_a_resource");

            canFollow.ShouldBeFalse();
        }

        [Theory]
        [InlineData("post_resource")]
        [InlineData("put_resource")]
        [InlineData("delete_resource")]
        public async Task can_follow_a_link_for_all_supported_http_verbs(string rel)
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);

            client = await client.Root();
            client = await client.Follow(rel);
            JToken json = await client.ResultAsJson();

            json.Value<string>("type").ShouldEqual(rel);
        }
    }

    public class SomeResource
    {
        public string Type { get; set; }
    }
}
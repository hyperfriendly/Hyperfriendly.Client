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

            await client.RootAsync();
            await client.FollowAsync("some_resource");
            JToken json = client.CurrentResult.ToJson();

            json.Value<string>("type").ShouldEqual("some_resource");
        }

        [Fact]
        public async Task can_post_content_to_a_resource()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);
            var content = new { foo = "bar" };

            await client.RootAsync();
            await client.FollowAsync("some_resource_with_content", content);
            JToken json = client.CurrentResult.ToJson();

            json.Value<string>("type").ShouldEqual("some_resource_with_content");
            json.Value<string>("foo").ShouldEqual("bar");
        }

        [Fact]
        public async Task can_get_a_resource_content_as_a_serialized_object()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);

            await client.RootAsync();
            await client.FollowAsync("some_resource");
            SomeResource resource = client.CurrentResult.ToObject<SomeResource>();

            resource.Type.ShouldEqual("some_resource");
        }

        [Fact]
        public async Task can_follow_is_true_when_rel_exists()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);
            await client.RootAsync();

            var canFollow = client.CanFollow("some_resource");

            canFollow.ShouldBeTrue();
        }

        [Fact]
        public async Task can_follow_is_true_when_rel_does_not_exist()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);
            await client.RootAsync();

            var canFollow = client.CanFollow("not_a_resource");

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

            await client.RootAsync();
            await client.FollowAsync(rel);
            JToken json = client.CurrentResult.ToJson();

            json.Value<string>("type").ShouldEqual(rel);
        }
    }

    public class SomeResource
    {
        public string Type { get; set; }
    }
}
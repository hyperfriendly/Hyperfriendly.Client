using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using HyperFriendly.Client.Tests.TestApi;
using Microsoft.Owin.Testing;
using Newtonsoft.Json.Linq;
using Should;
using Xunit;

namespace HyperFriendly.Client.Tests
{
    public class following_a_templated_link
    {
        [Fact]
        public async Task can_follow_a_templated_link_using_an_anonymus_object()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);
            await client.Root();

            await client.Follow("templated_resource", arguments: new { foo = "bar" });
            JToken json = await client.ResultAsJson();

            json.Value<string>("type").ShouldEqual("templated_resource");
        }

        [Fact]
        public async Task can_post_content_to_a_resource()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);
            var content = new { foo = "bar" };
            var arguments = new {foo = "bar"};
            await client.Root();
            await client.Follow("templated_resource_with_content", content, arguments);
            JToken json = await client.ResultAsJson();

            json.Value<string>("type").ShouldEqual("templated_resource_with_content");
            json.Value<string>("foo").ShouldEqual("bar");
        }
    }
}
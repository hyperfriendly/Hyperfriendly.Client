using System.Linq;
using System.Threading.Tasks;
using HyperFriendly.Client.Tests.TestApi;
using Microsoft.Owin.Testing;
using Should;
using Xunit;

namespace HyperFriendly.Client.Tests
{
    public class fetching_collections
    {
        [Fact]
        public async Task can_get_a_collection_as_a_serialized_enumerable()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);

            client = await client.Root();
            client = await client.Follow("collection_resource");
            var result = await client.CollectionResult<SomeResource>();

            var items = result.ToArray();
            items.Count().ShouldEqual(2);
            items.First().Type.ShouldEqual("some_resource");
            items.Last().Type.ShouldEqual("some_resource_2");
        }
    }
}
using System.Threading.Tasks;
using HyperFriendly.Client.Tests.TestApi;
using Microsoft.Owin.Testing;
using Should;
using Xunit;

namespace HyperFriendly.Client.Tests
{
    public class fetching_a_root_resource
    {
        [Fact]
        public async Task can_fetch_a_root_resource()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, "http://localhost:1337/");

            var result = await client.Root();

            result.CurrentResult.IsSuccessStatusCode.ShouldBeTrue("Actual: " + result.CurrentResult.StatusCode);
        }
    }
}
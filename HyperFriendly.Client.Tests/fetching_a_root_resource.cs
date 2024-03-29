﻿using System.Net;
using System.Threading.Tasks;
using HyperFriendly.Client.Tests.TestApi;
using Microsoft.Owin.Testing;
using Newtonsoft.Json.Linq;
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
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);

            await client.RootAsync();

            client.CurrentResult.StatusCode.ShouldEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task can_get_a_root_resource_content_as_json()
        {
            var testServer = TestServer.Create<StartUp>();
            var client = new HyperFriendlyHttpClient(testServer.HttpClient, Uris.Home);

            await client.RootAsync();
            JToken jsonResult = client.CurrentResult.ToJson();

            jsonResult
                .SelectToken("_links.self.href")
                .Value<string>()
                .ShouldEqual("/");
        }
    }
}
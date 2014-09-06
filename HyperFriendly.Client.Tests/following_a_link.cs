﻿using System.Threading.Tasks;
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
}
﻿using System.Threading.Tasks;
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
            client = await client.Root();

            client = await client.Follow("templated_resource", new { foo = "bar" });
            JToken json = await client.ResultAsJson();

            json.Value<string>("type").ShouldEqual("templated_resource");
        }
    }
}
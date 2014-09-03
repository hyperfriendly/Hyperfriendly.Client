using System.Net.Http.Headers;
using System.Web.Http;
using Owin;

namespace HyperFriendly.Client.Tests.TestApi
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("vnd/hyperfriendly+json"));
            app.UseWebApi(config);
        }
    }
}
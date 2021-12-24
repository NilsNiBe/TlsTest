using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Web.Http.SelfHost.Channels;
using Newtonsoft.Json.Serialization;

namespace TlsTest
{
  internal class SelfhostingServer
  {
    public static HttpSelfHostServer Server { get; private set; }

    public static void Run()
    {
      // netsh http add urlacl url=http://+:58000/ user=USERNAME listen=yes
      var uriBuilder = new UriBuilder($"https://{Dns.GetHostName()}") { Port = 58000 };
      var config = new HttpSelfHostConfiguration(uriBuilder.Uri) {};
      Configure(config);
      Server = new HttpSelfHostServer(config);
      Server.OpenAsync().Wait();
    }

    public static void ShutDown()
    {
      Server.CloseAsync().Wait();
    }

    private static void Configure(HttpSelfHostConfiguration config)
    {
      config.MaxConcurrentRequests = 1;

      var jsonFormatter = config.Formatters.JsonFormatter;
      jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

      config.Formatters.JsonFormatter.SupportedMediaTypes
        .Add(new MediaTypeHeaderValue("text/html"));

      config.MapHttpAttributeRoutes();
    }
  }
}

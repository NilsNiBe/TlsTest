using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace TlsTest
{
  internal class Program
  {
    public static int ServerPost { get; set; }
    public static string RequestUrl { get; set; }

    public static ConsoleKeyInfo Key { get; set; }

    static void Main(string[] args)
    {
      Console.WriteLine(RuntimeInformation.FrameworkDescription);
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
      Parser.Default.ParseArguments<CommandlineArgs>(args)
        .WithParsed(x =>
        {
          ServerPost = x.ServerPort;
          RequestUrl = x.RequestUrl;
        });

      Console.WriteLine("Start server");
      SelfhostingServer.Run();
      Console.WriteLine("Server started");
      Console.WriteLine("Press ESC to stop");
      var httpClient = new HttpClient();

      do
      {
        Key = Console.ReadKey();
        Console.WriteLine();
        if (Key.KeyChar == '1')
        {
          var res =  httpClient.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, RequestUrl + "/api/time")).Result;
          Console.WriteLine(res.RequestMessage);
          Console.WriteLine(res);
          Console.WriteLine(res.Content.ReadAsStringAsync().Result);
        }

      } while (Key.Key != ConsoleKey.Escape);


      Console.WriteLine("Close server");
      SelfhostingServer.ShutDown();
    }
  }
}

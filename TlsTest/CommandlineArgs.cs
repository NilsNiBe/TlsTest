using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace TlsTest
{
  public class CommandlineArgs
  {
    [Option('p', "Port", Required = false, HelpText = "Port des Servers (Default: 58000)")]
    public int ServerPort { get; set; } = 58000;

    [Option('h', "RequestUrl", Required = false, HelpText = "URL des HTTP-Requests")]
    public string RequestUrl { get; set; } = "https://192.168.178.63:58000";
  }
}

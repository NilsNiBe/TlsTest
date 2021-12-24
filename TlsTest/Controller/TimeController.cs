using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TlsTest.Controller
{

  [RoutePrefix("api/time")]
  public class TimeController : ApiController
  {
    [HttpGet]
    [Route]
    public async Task<DateTime> GetCurrentTime()
    {
      return DateTime.Now;
    }
  }

}

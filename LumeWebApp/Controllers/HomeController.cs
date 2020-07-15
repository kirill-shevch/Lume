using Microsoft.AspNetCore.Mvc;

namespace LumeWebApp.Controllers
{
	[ApiController]
    [Route("home")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "LumeApp";
        }

        [HttpGet]
        [Route("secure")]
        public ActionResult<string> SecureGet()
        {
            return "LumeApp";
        }
    }
}

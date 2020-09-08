using Microsoft.AspNetCore.Mvc;

namespace LumeWebApp.Controllers
{
	[ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
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

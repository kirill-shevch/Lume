using Microsoft.AspNetCore.Mvc;

namespace LumeWebApp.Controllers
{
	[ApiController]
    [Route("")]
    public class HomeController : Controller
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

        [HttpGet]
        [Route("license")]
        public ActionResult<string> License()
        {
            return View();
        }
    }
}

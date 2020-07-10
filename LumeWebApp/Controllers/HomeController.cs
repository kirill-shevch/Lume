using Microsoft.AspNetCore.Mvc;

namespace LumeWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "LumeApp";
        }
    }
}

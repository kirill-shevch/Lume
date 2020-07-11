using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LumeWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            var localConnectionString = _configuration[Constants.ConfigurationKeys.LocalConnectionString];
            var azureConnectionString = _configuration.GetConnectionString("AZURESQLCONNSTR_CONNECTIONSTRING");
            var azureSecret = _configuration["AZURESQLCONNSTR_CONNECTIONSTRING1"];

            return $"Local: {localConnectionString}\nAzure ConnectionString: {azureConnectionString}\nAzure Secret: {azureSecret}";
        }
    }
}

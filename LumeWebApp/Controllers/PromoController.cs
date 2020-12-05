using BLL.Core.Models.Promo;
using Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace LumeWebApp.Controllers
{
	[Route("api/promo")]
	[ApiController]
	public class PromoController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		public PromoController(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		[HttpGet]
		[Route("get-promo")]
		public ActionResult<ListOfPromo> GetProfile()
		{
			var listOfPromo = new ListOfPromo();
			listOfPromo.IsEventPromoRewardEnabled = _configuration.GetValue<bool?>(ConfigurationKeys.AzureIsEventPromoRewardEnabled) ?? _configuration.GetValue<bool>(ConfigurationKeys.IsEventPromoRewardEnabled);
			listOfPromo.IsNewYearPromoRewardEnabled = _configuration.GetValue<bool?>(ConfigurationKeys.AzureIsNewYearPromoRewardEnabled) ?? _configuration.GetValue<bool>(ConfigurationKeys.IsNewYearPromoRewardEnabled);
			return listOfPromo;
		}
	}
}

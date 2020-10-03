using BLL.Core.Interfaces;
using BLL.Core.Models.City;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LumeWebApp.Controllers
{
	[Route("api/city")]
	[ApiController]
	public class CityController : ControllerBase
	{
		private readonly ICityLogic _cityLogic;
		public CityController(ICityLogic cityLogic)
		{
			_cityLogic = cityLogic;
		}

		[HttpGet]
		[Route("get-cities")]
		public async Task<ActionResult<List<CityModel>>> GetCities()
		{
			return await _cityLogic.GetCities();
		}

		[HttpGet]
		[Route("check-city-for-promo-reward")]
		public async Task<ActionResult<CityPromoRewardModel>> CheckCityForPromoReward(long cityId)
		{
			return await _cityLogic.CheckCityForPromoReward(cityId);
		}
	}
}

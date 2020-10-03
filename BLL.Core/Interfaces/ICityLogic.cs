using BLL.Core.Models.City;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface ICityLogic
	{
		Task<List<CityModel>> GetCities();
		Task<CityPromoRewardModel> CheckCityForPromoReward(long cityId);
	}
}

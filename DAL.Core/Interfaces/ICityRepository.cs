using DAL.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Core.Interfaces
{
	public interface ICityRepository
	{
		Task<List<CityEntity>> GetCities();
		Task<CityEntity> GetCity(long cityId);
		Task<int> EventPromoInTheCitiesCount(List<string> cityNames);
	}
}

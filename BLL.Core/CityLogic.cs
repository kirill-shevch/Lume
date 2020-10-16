using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.City;
using DAL.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class CityLogic : ICityLogic
	{
		private readonly ICityRepository _cityRepository;
		private readonly IMapper _mapper;
		public CityLogic(ICityRepository cityRepository,
			IMapper mapper)
		{
			_cityRepository = cityRepository;
			_mapper = mapper;
		}

		public async Task<CityPromoRewardModel> CheckCityForPromoReward(long cityId)
		{
			var entity = await _cityRepository.GetCity(cityId);
			if (entity == null)
			{
				return new CityPromoRewardModel { IsCitySuitableForPromoReward = false };
			}
			switch (entity.CityName)
			{
				case "Москва":
				case "Санкт-Петербург":
				case "Саратов":
					{
						var eventsCount = await _cityRepository.EventPromoInTheCitiesCount(new List<string> { "Москва", "Санкт-Петербург", "Саратов" });
						var maxNumberOfCityPromoEvents = 500;
						
						return new CityPromoRewardModel { IsCitySuitableForPromoReward = eventsCount < maxNumberOfCityPromoEvents, 
							NumberOfCityPromoEvents = eventsCount,
							PromoEventsMaximum = maxNumberOfCityPromoEvents
						};
					}
				default:
					return new CityPromoRewardModel { IsCitySuitableForPromoReward = false };
			}
		}

		public async Task<List<CityModel>> GetCities()
		{
			var entities = await _cityRepository.GetCities();
			return _mapper.Map<List<CityModel>>(entities);
		}
	}
}

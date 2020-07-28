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
		public async Task<List<CityModel>> GetCities()
		{
			var entities = await _cityRepository.GetCities();
			return _mapper.Map<List<CityModel>>(entities);
		}
	}
}

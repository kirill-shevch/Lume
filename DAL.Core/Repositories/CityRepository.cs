using DAL.Core.Entities;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Core.Repositories
{
	public class CityRepository : ICityRepository
	{
		private readonly ICoreContextFactory _dbContextFactory;
		public CityRepository(ICoreContextFactory dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		public async Task<int> EventsInTheCityCount(long cityId)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.EventEntities.Where(x => x.CityId == cityId).CountAsync();
			}
		}

		public async Task<List<CityEntity>> GetCities()
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.CityEntities.ToListAsync();
			}
		}

		public async Task<CityEntity> GetCity(long cityId)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.CityEntities.SingleOrDefaultAsync(x => x.CityId == cityId);
			}
		}
	}
}

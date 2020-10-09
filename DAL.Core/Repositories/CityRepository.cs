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

		public async Task<int> EventsInTheCitiesCount(List<string> cityNames)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.EventEntities.Include(x => x.City).Where(x => x.City != null && cityNames.Contains(x.City.CityName)).CountAsync();
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

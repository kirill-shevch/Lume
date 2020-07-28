using DAL.Core.Entities;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Core.Repositories
{
	public class CityRepository : ICityRepository
	{
		private readonly CoreContextFactory _dbContextFactory;
		public CityRepository(CoreContextFactory dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		public async Task<List<CityEntity>> GetCities()
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.CityEntities.ToListAsync();
			}
		}
	}
}

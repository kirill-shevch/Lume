using Constants;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Core.Repositories
{
	public class BadgeRepository : IBadgeRepository
	{
		private readonly ICoreContextFactory _dbContextFactory;
		public BadgeRepository(ICoreContextFactory dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		public async Task AddBadgeToPerson(PersonEntity person, BadgeNames name)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var badge = await context.Badges.SingleOrDefaultAsync(x => x.BadgeId == (long)name);
				if (badge != null)
				{
					await context.PersonToBadgeEntities.AddAsync(new PersonToBadgeEntity { PersonId = person.PersonId, Badge = badge, IsViewed = false });
					await context.SaveChangesAsync();
				}
			}
		}

		public async Task<bool> AnyPersonUnviewedBadges(Guid personUid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonToBadgeEntities.Include(x => x.Person).AnyAsync(x => x.Person.PersonUid == personUid && !x.IsViewed);
			}
		}

		public async Task SetPersonBadgesViewed(Guid personUid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var personToBadgeEntities = await context.PersonToBadgeEntities.Include(x => x.Person).Where(x => x.Person.PersonUid == personUid && !x.IsViewed).ToListAsync();
				if (personToBadgeEntities != null && personToBadgeEntities.Any())
				{
					foreach (var entity in personToBadgeEntities)
					{
						entity.IsViewed = true;
					}
					context.PersonToBadgeEntities.UpdateRange(personToBadgeEntities);
					await context.SaveChangesAsync();
				}
			}
		}

		public async Task<List<PersonToBadgeEntity>> GetBadges(Guid personUid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonToBadgeEntities.Include(x => x.Person).Include(x => x.Badge).Where(x => x.Person.PersonUid == personUid).ToListAsync();
			}
		}

		public async Task<List<BadgeEntity>> GetBadges()
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.Badges.ToListAsync();
			}
		}
	}
}

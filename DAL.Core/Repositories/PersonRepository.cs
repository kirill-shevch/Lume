using DAL.Core.Entities;
using DAL.Core.Interfaces;
using DAL.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Core.Repositories
{
	public class PersonRepository : IPersonRepository
	{
		private readonly ICoreContextFactory _dbContextFactory;
		public PersonRepository(ICoreContextFactory dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		public async Task<PersonEntity> GetPerson(Guid uid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonEntities
					.Include(x => x.PersonImageContentEntity)
					.Include(x => x.FriendList)
						.ThenInclude(x => x.Friend)
							.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.City)
					.Include(x => x.SwipeHistory)
					.SingleOrDefaultAsync(x => x.PersonUid == uid, cancellationToken);
			}
		}

		public async Task CreatePerson(Guid personUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				await context.AddAsync(new PersonEntity { PersonUid = personUid }, cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task UpdatePerson(PersonEntity person, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				context.Update(person);
				await context.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task<bool> CheckPersonExistence(Guid personUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonEntities.AnyAsync(x => x.PersonUid == personUid, cancellationToken);
			}
		}

		public async Task<bool> CheckPersonFriendExistence(Guid personUid, Guid friendUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonFriendListEntities
					.AnyAsync(p => p.Person.PersonUid == personUid && p.Friend.PersonUid == friendUid);
			}
		}

		public async Task AddFriendToPerson(Guid personUid, Guid friendUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var person = await context.PersonEntities
					.FirstOrDefaultAsync(p => p.PersonUid == personUid);

				var friend = await context.PersonEntities
					.FirstOrDefaultAsync(p => p.PersonUid == friendUid);

				var personToFriendEntity = new PersonFriendListEntity { PersonId = person.PersonId, FriendId = friend.PersonId };

				await context.AddAsync(personToFriendEntity);

				await context.SaveChangesAsync();
			}
		}

		public async Task RemoveFriendFromPerson(Guid personUid, Guid friendUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var person = await context.PersonEntities
					.FirstOrDefaultAsync(p => p.PersonUid == personUid);

				var friend = await context.PersonEntities
					.FirstOrDefaultAsync(p => p.PersonUid == friendUid);

				var personToFriendEntity = await context.PersonFriendListEntities
					.FirstOrDefaultAsync(p => p.PersonId == person.PersonId);

				context.Remove(personToFriendEntity);

				await context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<PersonEntity>> GetPersonListByPage(Guid personUid, RepositoryGetPersonListFilter filter, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var personFriendList = await context.PersonFriendListEntities.Include(x => x.Person).Where(x => x.Person.PersonUid == personUid).Select(x => x.FriendId).ToListAsync();

				var query = context.PersonEntities
					.Include(x => x.PersonImageContentEntity)
					.Include(x => x.FriendList)
						.ThenInclude(x => x.Friend)
							.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.City)
					.AsNoTracking();

				query = query.Where(x => x.PersonUid != personUid);

				if (!string.IsNullOrWhiteSpace(filter.Query))
				{
					query = query.Where(p => (p.Name != null && p.Name.Contains(filter.Query) ||
						p.Login != null && p.Login.Contains(filter.Query)));
				}

				if (filter.CityId.HasValue)
				{
					query = query.Where(p => p.CityId == filter.CityId);
				}

				if (personFriendList.Any())
				{
					query = query.OrderByDescending(x => personFriendList.Contains(x.PersonId));
				}

				return await query.Skip(filter.PageSize * (filter.PageNumber - 1))
					.Take(filter.PageSize)
					.ToListAsync(cancellationToken);
			}
		}

		public async Task<List<PersonEntity>> GetAllPersonFriends(Guid personUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonFriendListEntities
					.Include(p => p.Friend)
						.ThenInclude(x => x.City)
					.Include(p => p.Friend)
						.ThenInclude(f => f.PersonImageContentEntity)
					.Where(p => p.Person.PersonUid == personUid)
					.Select(p => p.Friend)
					.ToListAsync(cancellationToken);
			}
		}

		public async Task<PersonEntity> GetRandomPerson(RepositoryRandomPersonFilter filter, long personId)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var query = context.PersonEntities.Include(x => x.Events).AsNoTracking();

				query = query.Where(x => x.PersonId != personId &&
					!x.Events.Any(x => x.EventId == filter.EventId) &&
					!filter.IgnoringPersonList.Contains(x.PersonId) &&
					!string.IsNullOrEmpty(x.Name) &&
					x.CityId.HasValue);
				
				if (filter.MinAge.HasValue)
				{
					query = query.Where(x => x.Age >= filter.MinAge.Value);
				}
				if (filter.MaxAge.HasValue)
				{
					query = query.Where(x => x.Age <= filter.MaxAge.Value);
				}
				if (filter.CityId.HasValue)
				{
					query = query.Where(x => x.CityId == filter.CityId);
				}

				var random = new Random();

				var personList = await query.Select(x => x.PersonId).ToListAsync();
				if (!personList.Any())
				{
					return null;
				}
				var randomPersonId = personList.ElementAt(random.Next(0, personList.Count()));

				return await context.PersonEntities
					.Include(x => x.PersonImageContentEntity)
					.Include(x => x.FriendList)
						.ThenInclude(x => x.Friend)
							.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.City)
					.SingleOrDefaultAsync(x => x.PersonId == randomPersonId);
			}
		}

		public async Task AddPersonSwipeHistoryRecord(PersonSwipeHistoryEntity entity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				await context.AddAsync(entity);
				await context.SaveChangesAsync();
			}
		}

		public async Task<bool> CheckPersonExistence(Guid personUid, string login, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonEntities.AnyAsync(x => personUid != x.PersonUid && x.Login == login, cancellationToken);
			}
		}

		public async Task RemovePersonImage(PersonImageContentEntity entity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				context.PersonImageContentEntities.Remove(entity);
				await context.SaveChangesAsync();
			}
		}
	}
}
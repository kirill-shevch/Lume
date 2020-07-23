using DAL.Core.Entities;
using DAL.Core.Interfaces;
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
		private readonly CoreContextFactory _dbContextFactory;
		public PersonRepository(CoreContextFactory dbContextFactory)
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

		public async Task<IEnumerable<PersonEntity>> GetPersonListByPage(int pageNumber, int pageSize, string filter = null, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var query = context.PersonEntities
					.Include(x => x.PersonImageContentEntity)
					.Include(x => x.FriendList)
						.ThenInclude(x => x.Friend)
							.ThenInclude(x => x.PersonImageContentEntity)
					.AsNoTracking();

				if (!string.IsNullOrWhiteSpace(filter))
				{
					query = query.Where(p => p.Name != null && p.Name.Contains(filter));
				}

				return await query.Skip(pageSize * (pageNumber - 1))
					.Take(pageSize)
					.ToListAsync(cancellationToken);
			}
		}
	}
}

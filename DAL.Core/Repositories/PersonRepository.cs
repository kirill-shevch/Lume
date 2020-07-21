using DAL.Core.Entities;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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
					.Where(p => p.Person.PersonUid == personUid && p.Friend.PersonUid == friendUid)
					.AnyAsync();
			}
		}

		public async Task AddFriendToPerson(Guid personUid, Guid friendUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var person = await context.PersonEntities
					.Where(p => p.PersonUid == personUid)
					.FirstOrDefaultAsync();

				var friend = await context.PersonEntities
					.Where(p => p.PersonUid == friendUid)
					.FirstOrDefaultAsync();

				var personToFriendEntity = new PersonFriendListEntity { PersonId = person.PersonId, FriendId = friend.PersonId };

				context.Add(personToFriendEntity);

				await context.SaveChangesAsync();
			}
		}

		public async Task RemoveFriendFromPerson(Guid personUid, Guid friendUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var person = await context.PersonEntities
					.Where(p => p.PersonUid == personUid)
					.FirstOrDefaultAsync();

				var friend = await context.PersonEntities
					.Where(p => p.PersonUid == friendUid)
					.FirstOrDefaultAsync();

				var personToFriendEntity = await context.PersonFriendListEntities
					.Where(p => p.PersonId == person.PersonId)
					.FirstOrDefaultAsync();

				context.Remove(personToFriendEntity);

				await context.SaveChangesAsync();
			}
		}
	}
}

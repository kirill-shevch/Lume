using DAL.Core.Entities;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Core.Repositories
{
	public class CoreRepository : ICoreRepository
	{
		private readonly CoreContextFactory _dbContextFactory;
		public CoreRepository(CoreContextFactory dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		public async Task<ChatEntity> GetChat(int id, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.ChatEntities
					.Include(x => x.ChatMessageEntities)
					.ThenInclude(x => x.ChatImageContentEntities)
					.SingleOrDefaultAsync(x => x.ChatId == id, cancellationToken);
			}
		}

		public async Task<EventEntity> GetEvent(int id, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.EventEntities
					.Include(x => x.EventStatus)
					.Include(x => x.EventType)
					.Include(x => x.EventImageContent)
					.Include(x => x.Administrator)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
					.SingleOrDefaultAsync(x => x.EventId == id, cancellationToken);
			}
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
	}
}

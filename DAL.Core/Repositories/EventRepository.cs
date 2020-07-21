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
	public class EventRepository : IEventRepository
	{
		private readonly CoreContextFactory _dbContextFactory;
		public EventRepository(CoreContextFactory dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		public async Task<bool> CheckEventExistence(Guid eventUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.EventEntities.AnyAsync(x => x.EventUid == eventUid, cancellationToken);
			}
		}

		public async Task CreateEvent(EventEntity eventEntity, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				await context.AddAsync(eventEntity, cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task<EventEntity> GetEvent(Guid eventUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.EventEntities
					.Include(x => x.EventStatus)
					.Include(x => x.EventType)
					.Include(x => x.EventImageContent)
					.Include(x => x.Administrator)
						.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
							.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.Chat)
					.SingleOrDefaultAsync(x => x.EventUid == eventUid, cancellationToken);
			}
		}

		public async Task<List<EventEntity>> GetEvents(Guid personUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.EventEntities
					.Include(x => x.EventStatus)
					.Include(x => x.EventType)
					.Include(x => x.EventImageContent)
					.Include(x => x.Administrator)
						.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
							.ThenInclude(x => x.PersonImageContentEntity)
					.Where(x => x.Administrator.PersonUid == personUid || x.Participants.Any(x => x.Person.PersonUid == personUid))
					.ToListAsync();
			}
		}

		public async Task UpdateEvent(EventEntity eventEntity, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				context.Update(eventEntity);
				await context.SaveChangesAsync(cancellationToken);
			}
		}
	}
}

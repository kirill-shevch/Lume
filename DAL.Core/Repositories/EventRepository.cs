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
					.Include(x => x.EventImageContentEntities)
					.Include(x => x.Administrator)
						.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
							.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.Chat)
					.Include(x => x.City)
					.Include(x => x.SwipeHistory)
					.SingleOrDefaultAsync(x => x.EventUid == eventUid, cancellationToken);
			}
		}

		public async Task<EventEntity> GetEventByChatId(long chatId, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var eventEntity = await context.EventEntities.Include(x => x.EventImageContentEntities).SingleOrDefaultAsync(x => x.ChatId == chatId);
				return eventEntity;
			}
		}

		public async Task<List<EventEntity>> GetEvents(Guid personUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.EventEntities
					.Include(x => x.EventStatus)
					.Include(x => x.EventType)
					.Include(x => x.EventImageContentEntities)
					.Include(x => x.Administrator)
						.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
							.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.Chat)
					.Include(x => x.City)
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

		public async Task<PersonToEventEntity> GetParticipant(Guid personUid, Guid eventUid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonToEventEntities
					.Include(x => x.Event)
					.Include(x => x.Person)
					.SingleOrDefaultAsync(x => x.Event.EventUid == eventUid && x.Person.PersonUid == personUid);
			}
		}

		public async Task RemoveParticipant(PersonToEventEntity entity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				context.Remove(entity);
				await context.SaveChangesAsync();
			}
		}

		public async Task AddParticipant(PersonToEventEntity entity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				await context.AddAsync(entity);
				await context.SaveChangesAsync();
			}
		}

		public async Task UpdateParticipant(PersonToEventEntity entity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				context.Update(entity);
				await context.SaveChangesAsync();
			}
		}

		public async Task<EventEntity> GetRandomEvent(RepositoryRandomEventFilter filter)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var query = context.EventEntities
					.Include(x => x.Administrator)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
					.AsNoTracking();

				query = query.Where(x => x.Administrator.PersonUid != filter.PersonUid &&
					!x.Participants.Any(x => x.Person.PersonUid == filter.PersonUid) &&
					(!x.MinAge.HasValue || x.MinAge <= filter.Age) &&
					(!x.MaxAge.HasValue || x.MaxAge >= filter.Age) &&
					!filter.IgnoringEventList.Contains(x.EventId));

				if (filter.PersonXCoordinate.HasValue && filter.PersonYCoordinate.HasValue && filter.Distance.HasValue)
				{
					query = query.Where(x => 
					filter.Distance >= Math.Sqrt(Math.Pow(Math.Abs(x.XCoordinate - filter.PersonXCoordinate.Value), 2) + Math.Pow(Math.Abs(x.YCoordinate - filter.PersonYCoordinate.Value), 2)));
				}
				if (filter.CityId.HasValue)
				{
					query = query.Where(x => x.CityId == filter.CityId);
				}
				var random = new Random();

				var events = await query.Select(x => x.EventId).ToListAsync();
				if (!events.Any())
				{
					return null;
				}
				var randomEventId = events.ElementAt(random.Next(0, events.Count()));
				return await context.EventEntities
					.Include(x => x.EventStatus)
					.Include(x => x.EventType)
					.Include(x => x.EventImageContentEntities)
					.Include(x => x.Administrator)
						.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
							.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.Chat)
					.Include(x => x.City)
					.SingleOrDefaultAsync(x => x.EventId == randomEventId);
			}
		}

		public async Task<List<EventEntity>> SearchForEvent(RepositoryEventSearchFilter repositoryFilter)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var query = context.EventEntities.AsNoTracking();
				if (!string.IsNullOrEmpty(repositoryFilter.Query))
				{
					query = query.Where(x => x.Name.Contains(repositoryFilter.Query) || x.Description.Contains(repositoryFilter.Query));
				}
				if (repositoryFilter.MinAge.HasValue)
				{
					query = query.Where(x => x.MinAge >= repositoryFilter.MinAge);
				}
				if (repositoryFilter.MaxAge.HasValue)
				{
					query = query.Where(x => x.MaxAge <= repositoryFilter.MaxAge);
				}
				if (repositoryFilter.StartTime.HasValue)
				{
					query = query.Where(x => x.StartTime.HasValue && x.StartTime == repositoryFilter.StartTime);
				}
				if (repositoryFilter.EndTime.HasValue)
				{
					query = query.Where(x => x.EndTime.HasValue && x.EndTime == repositoryFilter.EndTime);
				}
				if (repositoryFilter.Type.HasValue)
				{
					query = query.Where(x => x.EventTypeId == (long)repositoryFilter.Type);
				}
				if (repositoryFilter.Status.HasValue)
				{
					query = query.Where(x => x.EventStatusId == (long)repositoryFilter.Status);
				}
				if (repositoryFilter.IsOpenForInvitations.HasValue)
				{
					query = query.Where(x => x.IsOpenForInvitations == repositoryFilter.IsOpenForInvitations);
				}
				if (repositoryFilter.CityId.HasValue)
				{
					query = query.Where(x => x.CityId == repositoryFilter.CityId);
				}
				return await query.Include(x => x.EventImageContentEntities).Include(x => x.City).ToListAsync();
			}
		}

		public async Task AddEventSwipeHistoryRecord(EventSwipeHistoryEntity entity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				await context.AddAsync(entity);
				await context.SaveChangesAsync();
			}
		}
	}
}

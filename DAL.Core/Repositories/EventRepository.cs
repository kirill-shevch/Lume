﻿using Constants;
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
		private readonly ICoreContextFactory _dbContextFactory;
		public EventRepository(ICoreContextFactory dbContextFactory)
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
				foreach (var type in eventEntity.EventTypes)
				{
					type.EventType = await context.EventTypeEntities.SingleAsync(x => x.EventTypeId == type.EventTypeId);
				}
				await context.AddAsync(eventEntity, cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task<EventEntity> GetPureEvent(Guid eventUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.EventEntities.Include(x => x.Administrator).SingleOrDefaultAsync(x => x.EventUid == eventUid, cancellationToken);
			}
		}

		public async Task<EventEntity> GetEvent(Guid eventUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var entity = await context.EventEntities
					.Include(x => x.EventStatus)
					.Include(x => x.EventTypes)
						.ThenInclude(x => x.EventType)
					.Include(x => x.EventImageContentEntities)
					.Include(x => x.Chat)
					.Include(x => x.City)
					.Include(x => x.PromoRewardRequests)
					.SingleOrDefaultAsync(x => x.EventUid == eventUid, cancellationToken);
				if (entity != null)
				{
					await GetEventParticipants(context, entity);
				}
				return entity;
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
					.Include(x => x.EventTypes)
						.ThenInclude(x => x.EventType)
					.Include(x => x.EventImageContentEntities)
					.Include(x => x.Administrator)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
					.Include(x => x.Chat)
					.Include(x => x.City)
					.Where(x => x.Participants.Any(x => x.Person.PersonUid == personUid))
					.OrderBy(x => x.StartTime)
					.ToListAsync();
				
			}
		}

		public async Task UpdateEvent(EventEntity eventEntity, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				if (eventEntity.EventTypes != null)
				{
					foreach (var type in eventEntity.EventTypes)
					{
						type.EventType = await context.EventTypeEntities.SingleAsync(x => x.EventTypeId == type.EventTypeId);
					}
					await context.AddRangeAsync(eventEntity.EventTypes);
				}
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
					.Include(x => x.EventTypes)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
					.AsNoTracking();

				query = query.Where(x => x.Administrator.PersonUid != filter.PersonUid &&
					!x.Participants.Any(x => x.Person.PersonUid == filter.PersonUid) &&
					(!x.MinAge.HasValue || x.MinAge <= filter.Age) &&
					(!x.MaxAge.HasValue || x.MaxAge >= filter.Age) &&
					(x.EventStatusId == (long)EventStatus.Preparing || x.EventStatusId == (long)EventStatus.InProgress) &&
					!filter.IgnoringEventList.Contains(x.EventId));

				if (filter.PersonXCoordinate.HasValue && filter.PersonYCoordinate.HasValue && filter.Distance.HasValue)
				{
					query = query.Where(x =>
					(filter.Distance >= Math.Sqrt(Math.Pow(Math.Abs(x.XCoordinate - filter.PersonXCoordinate.Value), 2) + Math.Pow(Math.Abs(x.YCoordinate - filter.PersonYCoordinate.Value), 2))) || 
					(!filter.IsOnline.HasValue && x.IsOnline.Value));
				}
				if (filter.CityId.HasValue)
				{
					query = query.Where(x => (x.CityId == filter.CityId) ||
					(!filter.IsOnline.HasValue && x.IsOnline.Value));
				}
				if (filter.EventTypes != null && filter.EventTypes.Any())
				{
					query = query.Where(x => x.EventTypes.Any(x => filter.EventTypes.Contains(x.EventTypeId)));
				}
				if (filter.IsOpenForInvitations.HasValue)
				{
					query = query.Where(x => x.IsOpenForInvitations == filter.IsOpenForInvitations);
				}
				if (filter.IsOnline.HasValue)
				{
					query = query.Where(x => x.IsOnline == filter.IsOnline);
				}
				if (filter.MinimalStartTime.HasValue)
				{
					query = query.Where(x => x.StartTime.HasValue && x.StartTime.Value >= filter.MinimalStartTime.Value);
				}
				if (filter.MaximalEndTime.HasValue)
				{
					query = query.Where(x => x.StartTime.HasValue && x.StartTime.Value <= filter.MaximalEndTime.Value);
				}
				var random = new Random();

				var events = await query.Select(x => x.EventId).ToListAsync();
				if (!events.Any())
				{
					return null;
				}
				var randomEventId = events.ElementAt(random.Next(0, events.Count()));
				var entity = await context.EventEntities
					.Include(x => x.EventStatus)
					.Include(x => x.EventTypes)
						.ThenInclude(x => x.EventType)
					.Include(x => x.EventImageContentEntities)
					.Include(x => x.Chat)
					.Include(x => x.City)
					.SingleOrDefaultAsync(x => x.EventId == randomEventId);
				if (entity != null)
				{
					await GetEventParticipants(context, entity);
				}
				return entity;
			}
		}

		public async Task<List<EventEntity>> SearchForEvent(RepositoryEventSearchFilter repositoryFilter)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var query = context.EventEntities.Include(x => x.EventTypes).AsNoTracking();
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
					query = query.Where(x => x.EventTypes.Any(x => x.EventTypeId == (long)repositoryFilter.Type));
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
					query = query.Where(x => (x.CityId == repositoryFilter.CityId) ||
					(!repositoryFilter.IsOnline.HasValue && x.IsOnline.Value));
				}
				if (repositoryFilter.IsOnline.HasValue)
				{
					query = query.Where(x => x.IsOnline == repositoryFilter.IsOnline);
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

		public async Task<List<EventEntity>> TransferEventsStatuses(CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var date = DateTime.UtcNow;
				var inProgressEvents = context.EventEntities.Where(x => x.StartTime < date && x.EndTime > date && x.EventStatusId == (long)EventStatus.Preparing);
				await inProgressEvents.ForEachAsync(x => x.EventStatusId = (long)EventStatus.InProgress, cancellationToken);
				var endedEvents = context.EventEntities.Where(x => x.EndTime < date && (x.EventStatusId == (long)EventStatus.InProgress || x.EventStatusId == (long)EventStatus.Preparing));
				await endedEvents.ForEachAsync(x => x.EventStatusId = (long)EventStatus.Ended, cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
				var listOfEvents = await endedEvents
					.Include(x => x.Administrator)
						.ThenInclude(x => x.Badges)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
							.ThenInclude(x => x.Badges)
					.Include(x => x.EventTypes)
					.ToListAsync();
				return listOfEvents;
			}
		}

		public async Task RemoveOutdatedParticipants(CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var outdatedParticipants = context.PersonToEventEntities.Include(x => x.Event)
					.Where(x => (x.Event.EventStatusId == (long)EventStatus.Ended || x.Event.EventStatusId == (long)EventStatus.Canceled) &&
					(x.ParticipantStatusId == (long)ParticipantStatus.WaitingForApproveFromEvent || x.ParticipantStatusId == (long)ParticipantStatus.WaitingForApproveFromUser));
				await outdatedParticipants.ForEachAsync(x => 
				{ 
					x.ParticipantStatusId = (long)ParticipantStatus.Rejected; 
					x.Event = null; 
				}, cancellationToken);
				context.PersonToEventEntities.UpdateRange(outdatedParticipants);
				await context.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task RemoveEventTypes(long eventId)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var entities = context.EventTypeToEventEntities.Where(x => x.EventId == eventId);
				context.RemoveRange(entities);
				await context.SaveChangesAsync();
			}
		}

		public async Task<List<EventEntity>> GetPersonInvitations(Guid uid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonToEventEntities
					.Include(x => x.Event)
					.Include(x => x.Person)
					.Where(x => x.ParticipantStatusId == (long)ParticipantStatus.WaitingForApproveFromUser && x.Person.PersonUid == uid)
					.Select(x => x.Event)
					.ToListAsync();
			}
		}

		public async Task AddPromoRewardRequest(PromoRewardRequestEntity entity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				await context.AddAsync(entity);
				await context.SaveChangesAsync();
			}
		}

		public async Task RemoveEventImage(EventImageContentEntity entity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				context.Remove(entity);
				await context.SaveChangesAsync();
			}
		}

		public async Task<List<EventEntity>> GetListOfLatestEvents(TimeSpan borderTime, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var date = DateTime.UtcNow + borderTime;
				return await context.EventEntities
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Event)
					.Where(x => x.StartTime < date && 
						x.EventStatusId == (long)EventStatus.Preparing && 
						!x.IsPrelaunchNotificationSent)
					.ToListAsync();
			}
		}

		public async Task SetPrelaunchNotificationsFlag(List<EventEntity> eventEntities, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				foreach (var entity in eventEntities)
				{
					entity.IsPrelaunchNotificationSent = true;
					entity.Participants = null;
				}
				context.UpdateRange(eventEntities);
				await context.SaveChangesAsync();
			}
		}

		public async Task AddReport(EventReportEntity reportEntity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				await context.EventReportEntities.AddAsync(reportEntity);
				await context.SaveChangesAsync();
			}
		}

		public async Task<List<long>> GetEventSwipeHistory(long eventId)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.EventSwipeHistoryEntities.Where(x => x.EventId == eventId)
					.Select(x => x.PersonId)
					.ToListAsync();
			}
		}

		private async Task GetEventParticipants(CoreDbContext context, EventEntity entity)
		{
			var participants = await context.PersonToEventEntities
				.Include(x => x.Person)
					.ThenInclude(x => x.City)
				.Include(x => x.Person)
					.ThenInclude(x => x.PersonImageContentEntity)
				.Where(x => x.EventId == entity.EventId)
				.ToListAsync();
			entity.Participants = participants;
			entity.Administrator = participants.Single(x => x.PersonId == entity.AdministratorId).Person;
		}
	}
}

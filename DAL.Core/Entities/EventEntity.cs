using System;
using System.Collections.Generic;

namespace DAL.Core.Entities
{
	public class EventEntity
	{
		public const string TableName = "Event";
		public long EventId { get; set; }
		public Guid EventUid { get; set; }
		public string Name { get; set; }
		public byte? MinAge { get; set; }
		public byte? MaxAge { get; set; }
		public double XCoordinate { get; set; }
		public double YCoordinate { get; set; }
		public string Description { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public bool? IsOpenForInvitations { get; set; }
		public bool? IsOnline { get; set; }
		public long EventStatusId { get; set; }
		public long AdministratorId { get; set; }
		public long ChatId { get; set; }
		public long? CityId { get; set; }
		public IEnumerable<EventImageContentEntity> EventImageContentEntities { get; set; }
		public EventStatusEntity EventStatus { get; set; }
		public PersonEntity Administrator { get; set; }
		public ChatEntity Chat { get; set; }
		public IEnumerable<PersonToEventEntity> Participants { get; set; }
		public IEnumerable<EventSwipeHistoryEntity> SwipeHistory { get; set; }
		public IEnumerable<EventTypeToEventEntity> EventTypes { get; set; }
		public IEnumerable<PromoRewardRequestEntity> PromoRewardRequests { get; set; }
		public CityEntity City { get; set; }
	}
}
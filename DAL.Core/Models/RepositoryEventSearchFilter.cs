using Constants;
using System;

namespace DAL.Core.Models
{
	public class RepositoryEventSearchFilter
	{
		public string Query { get; set; }
		public byte? MinAge { get; set; }
		public byte? MaxAge { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public EventType? Type { get; set; }
		public EventStatus? Status { get; set; }
		public bool? IsOpenForInvitations { get; set; }
		public long? CityId { get; set; }
	}
}

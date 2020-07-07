using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Authorization.Entities
{
	public class UserAuthEntity
	{
		[Key]
		public int UserId { get; set; }
		public Guid UserUid { get; set; }
		public string PhoneNumber { get; set; }
		public string AccessKey { get; set; }
		public string RefreshKey { get; set; }
		public DateTime? ExpirationTime { get; set; }
		public string TemporaryCode { get; set; }
	}
}

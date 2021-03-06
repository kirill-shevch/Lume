﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Authorization.Entities
{
	[Table("PersonAuth")]
	public class PersonAuthEntity
	{
		[Key]
		public long PersonAuthId { get; set; }
		public Guid PersonUid { get; set; }
		public string PhoneNumber { get; set; }
		public string AccessKey { get; set; }
		public string RefreshKey { get; set; }
		public DateTime? ExpirationTime { get; set; }
		public string TemporaryCode { get; set; }
		public DateTime? TemporaryCodeTime { get; set; }
		public bool IsBlocked { get; set; }
		public DateTime? CreationTime { get; set; }
	}
}

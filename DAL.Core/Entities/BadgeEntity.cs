﻿using System;

namespace DAL.Core.Entities
{
	public class BadgeEntity
	{
		public const string TableName = "Badge";

		public long BadgeId { get; set; }
		public Guid BadgeUid { get; set; }
		public string Name { get; set; }
	}
}

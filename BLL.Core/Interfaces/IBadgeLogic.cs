using BLL.Core.Models.Badge;
using DAL.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface IBadgeLogic
	{
		Task<List<BadgeModel>> GetBadges(Guid personUid);
		Task AddBadges(List<EventEntity> eventEntities);
	}
}

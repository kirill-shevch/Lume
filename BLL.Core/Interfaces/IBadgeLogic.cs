using BLL.Core.Models.Badge;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface IBadgeLogic
	{
		Task<List<BadgeModel>> GetBadges(Guid personUid);
	}
}

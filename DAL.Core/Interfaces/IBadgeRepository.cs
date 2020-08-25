using Constants;
using DAL.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Core.Interfaces
{
	public interface IBadgeRepository
	{
		Task AddBadgeToPerson(Guid personUid, BadgeNames name);

		Task<bool> AnyPersonUnviewedBadges(Guid personUid);

		Task SetPersonBadgesViewed(Guid personUid);

		Task<List<BadgeEntity>> GetBadges(Guid personUid);
	}
}

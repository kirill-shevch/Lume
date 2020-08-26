using Constants;
using DAL.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Core.Interfaces
{
	public interface IBadgeRepository
	{
		Task AddBadgeToPerson(PersonEntity person, BadgeNames name);
		Task<bool> AnyPersonUnviewedBadges(Guid personUid);
		Task SetPersonBadgesViewed(Guid personUid);
		Task<List<PersonToBadgeEntity>> GetBadges(Guid personUid);
		Task<List<BadgeEntity>> GetBadges();
	}
}

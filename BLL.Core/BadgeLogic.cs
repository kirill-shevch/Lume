using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Badge;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class BadgeLogic : IBadgeLogic
	{
		private readonly IBadgeRepository _badgeRepository;
		private readonly IMapper _mapper;
		public BadgeLogic(IBadgeRepository badgeRepository,
			IMapper mapper)
		{
			_badgeRepository = badgeRepository;
			_mapper = mapper;
		}

		public async Task<List<BadgeModel>> GetBadges(Guid personUid)
		{
			var personToBadgeEntities = await _badgeRepository.GetBadges(personUid);
			var models = _mapper.Map<List<BadgeModel>>(personToBadgeEntities);
			if (models.Any(x => !x.IsViewed))
			{
				await _badgeRepository.SetPersonBadgesViewed(personUid);
			}
			return models;
		}
	}
}

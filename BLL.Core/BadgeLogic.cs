using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Badge;
using Constants;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace BLL.Core
{
	public class BadgeLogic : IBadgeLogic
	{
		protected readonly IHttpContextAccessor _contextAccessor;
		private readonly IBadgeRepository _badgeRepository;
		private readonly IMapper _mapper;
		public BadgeLogic(IBadgeRepository badgeRepository,
			IMapper mapper,
			IHttpContextAccessor contextAccessor)
		{
			_badgeRepository = badgeRepository;
			_mapper = mapper;
			_contextAccessor = contextAccessor;
		}

		public async Task<List<BadgeModel>> GetBadges(Guid personUid)
		{
			var personToBadgeEntities = await _badgeRepository.GetBadges(personUid);
			var badges = await _badgeRepository.GetAllBadges();
			var models = _mapper.Map<List<BadgeModel>>(badges);
			var httpContext = _contextAccessor.HttpContext;
			var culture = CultureParser.GetCultureFromHttpContext(httpContext);
			foreach (var model in models)
			{
				model.Name = BadgeText.GetBadgeText(model.BadgeName, BadgeTextType.Name, culture);
				model.Description = BadgeText.GetBadgeText(model.BadgeName, BadgeTextType.Description, culture);
				model.Received = personToBadgeEntities.Any(x => x.BadgeId == (long)model.BadgeName);
			}
			return models;
		}

		public async Task AddBadges(List<EventEntity> eventEntities)
		{
			foreach (var eventEntity in eventEntities)
			{
				if (!eventEntity.Administrator.Badges.Any(x => x.BadgeId == (long)BadgeNames.CreatedEvent))
					await _badgeRepository.AddBadgeToPerson(eventEntity.Administrator, BadgeNames.CreatedEvent);

				foreach (var participant in eventEntity.Participants)
				{
					if (!participant.Person.Badges.Any(x => x.BadgeId == (long)BadgeNames.ParticipatedInEvent))
						await _badgeRepository.AddBadgeToPerson(participant.Person, BadgeNames.ParticipatedInEvent);
					foreach (var type in eventEntity.EventTypes)
					{
						switch (type.EventTypeId)
						{
							case (long)EventType.Party:
								if (!participant.Person.Badges.Any(x => x.BadgeId == (long)BadgeNames.ParticipatedInParty))
									await _badgeRepository.AddBadgeToPerson(participant.Person, BadgeNames.ParticipatedInParty);
								break;
							case (long)EventType.Culture:
								if (!participant.Person.Badges.Any(x => x.BadgeId == (long)BadgeNames.ParticipatedInCulture))
									await _badgeRepository.AddBadgeToPerson(participant.Person, BadgeNames.ParticipatedInCulture);
								break;
							case (long)EventType.Sport:
								if (!participant.Person.Badges.Any(x => x.BadgeId == (long)BadgeNames.ParticipatedInSport))
									await _badgeRepository.AddBadgeToPerson(participant.Person, BadgeNames.ParticipatedInSport);
								break;
							case (long)EventType.Nature:
								if (!participant.Person.Badges.Any(x => x.BadgeId == (long)BadgeNames.ParticipatedInNature))
									await _badgeRepository.AddBadgeToPerson(participant.Person, BadgeNames.ParticipatedInNature);
								break;
							case (long)EventType.Communication:
								if (!participant.Person.Badges.Any(x => x.BadgeId == (long)BadgeNames.ParticipatedInCommunication))
									await _badgeRepository.AddBadgeToPerson(participant.Person, BadgeNames.ParticipatedInCommunication);
								break;
							case (long)EventType.Game:
								if (!participant.Person.Badges.Any(x => x.BadgeId == (long)BadgeNames.ParticipatedInGame))
									await _badgeRepository.AddBadgeToPerson(participant.Person, BadgeNames.ParticipatedInGame);
								break;
							case (long)EventType.Study:
								if (!participant.Person.Badges.Any(x => x.BadgeId == (long)BadgeNames.ParticipatedInStudy))
									await _badgeRepository.AddBadgeToPerson(participant.Person, BadgeNames.ParticipatedInStudy);
								break;
							case (long)EventType.Food:
								if (!participant.Person.Badges.Any(x => x.BadgeId == (long)BadgeNames.ParticipatedInFood))
									await _badgeRepository.AddBadgeToPerson(participant.Person, BadgeNames.ParticipatedInFood);
								break;
							case (long)EventType.Concert:
								if (!participant.Person.Badges.Any(x => x.BadgeId == (long)BadgeNames.ParticipatedInConcert))
									await _badgeRepository.AddBadgeToPerson(participant.Person, BadgeNames.ParticipatedInConcert);
								break;
							case (long)EventType.Travel:
								if (!participant.Person.Badges.Any(x => x.BadgeId == (long)BadgeNames.ParticipatedInTravel))
									await _badgeRepository.AddBadgeToPerson(participant.Person, BadgeNames.ParticipatedInTravel);
								break;
							default:
								break;
						}
					}
				}
			}
		}
	}
}

using System.Collections.Generic;

namespace Constants
{
	public enum BadgeTextType
	{
		Name,
		Description,
	}
	public static class BadgeText
	{
		private static Dictionary<(BadgeNames, BadgeTextType, string), string> MessageContent = new Dictionary<(BadgeNames, BadgeTextType, string), string>
		{
			[(BadgeNames.ParticipatedInEvent, BadgeTextType.Name, "en-US")] = "Newbie",
			[(BadgeNames.ParticipatedInEvent, BadgeTextType.Name, "ru-RU")] = "Новобранец",
			[(BadgeNames.ParticipatedInEvent, BadgeTextType.Description, "en-US")] = "For participating in the first event",
			[(BadgeNames.ParticipatedInEvent, BadgeTextType.Description, "ru-RU")] = "За участие в первом событии",

			[(BadgeNames.CreatedEvent, BadgeTextType.Name, "en-US")] = "Creator",
			[(BadgeNames.CreatedEvent, BadgeTextType.Name, "ru-RU")] = "Организатор",
			[(BadgeNames.CreatedEvent, BadgeTextType.Description, "en-US")] = "For the first organized event",
			[(BadgeNames.CreatedEvent, BadgeTextType.Description, "ru-RU")] = "За первое организованное событие",

			[(BadgeNames.ParticipatedInParty, BadgeTextType.Name, "en-US")] = "Life of the party",
			[(BadgeNames.ParticipatedInParty, BadgeTextType.Name, "ru-RU")] = "Душа компании",
			[(BadgeNames.ParticipatedInParty, BadgeTextType.Description, "en-US")] = "For participating in the party",
			[(BadgeNames.ParticipatedInParty, BadgeTextType.Description, "ru-RU")] = "За участие в вечеринке",

			[(BadgeNames.ParticipatedInCulture, BadgeTextType.Name, "en-US")] = "Esthete",
			[(BadgeNames.ParticipatedInCulture, BadgeTextType.Name, "ru-RU")] = "Эстет",
			[(BadgeNames.ParticipatedInCulture, BadgeTextType.Description, "en-US")] = "For participation in a cultural event",
			[(BadgeNames.ParticipatedInCulture, BadgeTextType.Description, "ru-RU")] = "За участие в культурном мероприятии",

			[(BadgeNames.ParticipatedInSport, BadgeTextType.Name, "en-US")] = "Need to pump up",
			[(BadgeNames.ParticipatedInSport, BadgeTextType.Name, "ru-RU")] = "Надо подкачаться",
			[(BadgeNames.ParticipatedInSport, BadgeTextType.Description, "en-US")] = "For participation in a sporting event",
			[(BadgeNames.ParticipatedInSport, BadgeTextType.Description, "ru-RU")] = "За участие в спортивном событии",

			[(BadgeNames.ParticipatedInNature, BadgeTextType.Name, "en-US")] = "Naturalist",
			[(BadgeNames.ParticipatedInNature, BadgeTextType.Name, "ru-RU")] = "Натуралист",
			[(BadgeNames.ParticipatedInNature, BadgeTextType.Description, "en-US")] = "For participating in an outdoor event",
			[(BadgeNames.ParticipatedInNature, BadgeTextType.Description, "ru-RU")] = "За участие в событии на природе",

			[(BadgeNames.ParticipatedInCommunication, BadgeTextType.Name, "en-US")] = "Sociable",
			[(BadgeNames.ParticipatedInCommunication, BadgeTextType.Name, "ru-RU")] = "Коммуникабельный",
			[(BadgeNames.ParticipatedInCommunication, BadgeTextType.Description, "en-US")] = "For craving for communication",
			[(BadgeNames.ParticipatedInCommunication, BadgeTextType.Description, "ru-RU")] = "За тягу к общению",

			[(BadgeNames.ParticipatedInGame, BadgeTextType.Name, "en-US")] = "Gamer",
			[(BadgeNames.ParticipatedInGame, BadgeTextType.Name, "ru-RU")] = "Геймер",
			[(BadgeNames.ParticipatedInGame, BadgeTextType.Description, "en-US")] = "For killing dragons",
			[(BadgeNames.ParticipatedInGame, BadgeTextType.Description, "ru-RU")] = "За убийство драконов",

			[(BadgeNames.ParticipatedInStudy, BadgeTextType.Name, "en-US")] = "Brain",
			[(BadgeNames.ParticipatedInStudy, BadgeTextType.Name, "ru-RU")] = "Острый ум",
			[(BadgeNames.ParticipatedInStudy, BadgeTextType.Description, "en-US")] = "For craving for knowledge",
			[(BadgeNames.ParticipatedInStudy, BadgeTextType.Description, "ru-RU")] = "За тягу к знаниям",

			[(BadgeNames.ParticipatedInFood, BadgeTextType.Name, "en-US")] = "Gourmet",
			[(BadgeNames.ParticipatedInFood, BadgeTextType.Name, "ru-RU")] = "Гурман",
			[(BadgeNames.ParticipatedInFood, BadgeTextType.Description, "en-US")] = "For matcha lattes, meetballs and falafels",
			[(BadgeNames.ParticipatedInFood, BadgeTextType.Description, "ru-RU")] = "За дегустацию матча-латте, митболлов и фалафелей",

			[(BadgeNames.ParticipatedInConcert, BadgeTextType.Name, "en-US")] = "Spectator",
			[(BadgeNames.ParticipatedInConcert, BadgeTextType.Name, "ru-RU")] = "Зритель",
			[(BadgeNames.ParticipatedInConcert, BadgeTextType.Description, "en-US")] = "For participating in a performance or concert",
			[(BadgeNames.ParticipatedInConcert, BadgeTextType.Description, "ru-RU")] = "За участие в выступлении или концерте",

			[(BadgeNames.ParticipatedInTravel, BadgeTextType.Name, "en-US")] = "Wanderer",
			[(BadgeNames.ParticipatedInTravel, BadgeTextType.Name, "ru-RU")] = "Странник",
			[(BadgeNames.ParticipatedInTravel, BadgeTextType.Description, "en-US")] = "For participation in the trip",
			[(BadgeNames.ParticipatedInTravel, BadgeTextType.Description, "ru-RU")] = "За участие в путешествии",
		};

		public static string GetBadgeText(BadgeNames name, BadgeTextType type, string culture) => MessageContent[(name, type, culture)];
	}
}

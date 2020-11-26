using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Person;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using DAL.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class PersonLogic : IPersonLogic
	{
		private readonly IPersonRepository _personRepository;
		private readonly IEventRepository _eventRepository;
		private readonly IChatRepository _chatRepository;
		private readonly IBadgeRepository _badgeRepository;
		private readonly IImageLogic _imageLogic;
		private readonly IMapper _mapper;

		private const int countOfFriends = 5;

		public PersonLogic(IPersonRepository personRepository,
			IEventRepository eventRepository,
			IChatRepository chatRepository,
			IBadgeRepository badgeRepository,
			IImageLogic imageLogic,
			IMapper mapper)
		{
			_personRepository = personRepository;
			_eventRepository = eventRepository;
			_chatRepository = chatRepository;
			_badgeRepository = badgeRepository;
			_imageLogic = imageLogic;
			_mapper = mapper;
		}

		public async Task CreatePerson(Guid personUid)
		{
			var personExists = await _personRepository.CheckPersonExistence(personUid);
			if (!personExists)
			{
				await _personRepository.CreatePerson(personUid);
			}
		}

		public async Task<PersonModel> GetPerson(Guid personUid)
		{
			var entity = await _personRepository.GetPerson(personUid);
			var model = _mapper.Map<PersonModel>(entity);
			model.Friends = entity.FriendList.Select(x => _mapper.Map<PersonModel>(x.Friend)).Take(countOfFriends).ToList();
			foreach (var friend in model.Friends)
			{
				friend.IsFriend = true;
			}
			return model;
		}

		public async Task<PersonModel> UpdatePerson(UpdatePersonModel updatePersonModel, Guid personUid)
		{
			var entity = await _personRepository.GetPerson(personUid);
			if (!string.IsNullOrEmpty(updatePersonModel.Name))
				entity.Name = updatePersonModel.Name;
			if (!string.IsNullOrEmpty(updatePersonModel.Description))
				entity.Description = updatePersonModel.Description;
			if (updatePersonModel.Age.HasValue)
				entity.Age = updatePersonModel.Age;
			if (updatePersonModel.CityId.HasValue)
				entity.CityId = updatePersonModel.CityId;
			if (!string.IsNullOrEmpty(updatePersonModel.Login))
				entity.Login = updatePersonModel.Login;
			if (string.IsNullOrWhiteSpace(entity.Login) && string.IsNullOrWhiteSpace(updatePersonModel.Login))
			{
				entity.Login = await GenarateLogin(updatePersonModel.Name);
			}
			if (updatePersonModel.Token != null)
			{
				entity.Token = updatePersonModel.Token;
				await _personRepository.RemoveTokenForEveryPerson(updatePersonModel.Token);
			}
			var deleteOldImage = false;
			var imageToDelete = entity.PersonImageContentEntity;
			if (updatePersonModel.Image != null)
			{
				deleteOldImage = entity.PersonImageContentEntity != null;
				var imageUid = await _imageLogic.SaveImage(updatePersonModel.Image);
				var miniImageUid = await _imageLogic.SaveImage(updatePersonModel.MiniImage);
				entity.PersonImageContentEntity = new PersonImageContentEntity { PersonImageContentUid = imageUid, PersonMiniatureImageContentUid = miniImageUid };
			}
			else
			{
				entity.PersonImageContentEntity = null;
			}
			var model = _mapper.Map<PersonModel>(entity);
			entity.FriendList = null;
			entity.City = null;
			entity.SwipeHistory = null;
			await _personRepository.UpdatePerson(entity);

			if (deleteOldImage)
			{
				await _personRepository.RemovePersonImage(imageToDelete);
				await _imageLogic.RemoveImage(imageToDelete.PersonImageContentUid);
				if (imageToDelete.PersonMiniatureImageContentUid.HasValue)
				{
					await _imageLogic.RemoveImage(imageToDelete.PersonMiniatureImageContentUid.Value);
				}
			}
			return model;
		}

		private async Task<string> GenarateLogin(string name)
		{
			var login = RussianTransliteration.RussianTransliterator.GetTransliteration(name.ToLower());
			var loginList = await _personRepository.GetLoginList(login);
			var loginNumber = 0;
			foreach (var item in loginList)
			{
				if (item.Length > login.Length && char.IsDigit(item, login.Length))
				{
					int number;
					if (int.TryParse(item.Substring(login.Length), out number))
					{
						loginNumber = loginNumber < number ? number : loginNumber;
					}
				}
			}
			loginNumber++;
			return login + loginNumber;
		}

		public async Task<bool> IsPersonFilledUp(Guid personUid)
		{
			var entity = await _personRepository.GetPerson(personUid);
			return entity != null &&
				!string.IsNullOrEmpty(entity.Name) &&
				entity.Age.HasValue &&
				!string.IsNullOrEmpty(entity.Login) &&
				entity.PersonImageContentEntity != null;
		}

		public async Task AddFriendToPerson(Guid personUid, Guid friendUid)
		{
			await _personRepository.AddFriendToPerson(personUid, friendUid, true);
			if (!(await _personRepository.CheckPersonFriendExistence(friendUid, personUid)))
			{
				await _personRepository.AddFriendToPerson(friendUid, personUid, false);
			}
		}

		public async Task RemoveFriendFromPerson(Guid personUid, Guid friendUid)
		{
			await _personRepository.RemoveFriendFromPerson(personUid, friendUid);
		}

		public async Task<IEnumerable<PersonModel>> GetPersonListByPage(Guid personUid, GetPersonListFilter model)
		{
			var filter = _mapper.Map<RepositoryGetPersonListFilter>(model);
			var persons = await _personRepository.GetPersonListByPage(personUid, filter);
			var notApprovedFriends = await _personRepository.GetNewFriends(personUid);
			var personModels = _mapper.Map<IEnumerable<PersonModel>>(persons);
			foreach (var personModel in personModels)
			{
				personModel.FriendshipApprovalRequired = notApprovedFriends.Any(x => x.PersonUid == personModel.PersonUid);
				personModel.IsFriend = await _personRepository.CheckPersonFriendExistence(personUid, personModel.PersonUid);
			}
			return personModels.OrderByDescending(x => x.FriendshipApprovalRequired).ThenByDescending(x => x.IsFriend);
		}

		public async Task<bool> CheckFriendship(Guid personUid, Guid friendUid)
		{
			return await _personRepository.CheckPersonFriendExistence(personUid, friendUid);
		}

		public async Task<List<PersonModel>> GetAllPersonFriends(Guid personUid)
		{
			var entities = await _personRepository.GetAllPersonFriends(personUid);
			var notApprovedFriends = await _personRepository.GetNewFriends(personUid);
			var models = _mapper.Map<List<PersonModel>>(entities);
			foreach (var model in models)
			{
				model.IsFriend = true;
				model.FriendshipApprovalRequired = notApprovedFriends.Any(x => x.PersonUid == model.PersonUid);
			}
			return models.OrderByDescending(x => x.FriendshipApprovalRequired).ToList();
		}

		public async Task<PersonModel> GetRandomPerson(RandomPersonFilter randomPersonFilter, Guid uid)
		{
			var personEntity = await _personRepository.GetPerson(uid);
			var eventEntity = await _eventRepository.GetPureEvent(randomPersonFilter.EventUid);
			var filter = _mapper.Map<RepositoryRandomPersonFilter>(randomPersonFilter);
			filter.EventId = eventEntity.EventId;
			filter.IgnoringPersonList = await _eventRepository.GetEventSwipeHistory(eventEntity.EventId);
			var randomPersonEntity = await _personRepository.GetRandomPerson(filter, personEntity.PersonId);
			return _mapper.Map<PersonModel>(randomPersonEntity);
		}

		public async Task AddPersonSwipeHistory(Guid eventUid, Guid personUid)
		{
			var eventEntity = await _eventRepository.GetPureEvent(eventUid);
			var personEntity = await _personRepository.GetPerson(personUid);
			await _personRepository.AddPersonSwipeHistoryRecord(new PersonSwipeHistoryEntity { PersonId = personEntity.PersonId, EventId = eventEntity.EventId });
		}

		public async Task ConfirmFriend(Guid uid, Guid friendGuid)
		{
			await _personRepository.ConfirmFriend(uid, friendGuid);
		}

		public async Task<PersonNotificationsModel> GetPersonNotifications(Guid uid)
		{
			var model = new PersonNotificationsModel();
			model.NewEventInvitationsCount = (await _eventRepository.GetPersonInvitations(uid)).Count;
			model.NewFriendsCount = (await _personRepository.GetNewFriends(uid)).Count;
			model.AnyNewChatMessages = await _chatRepository.CheckPersonForNewChatMessages(uid);
			model.AnyNewBadges = await _badgeRepository.AnyPersonUnviewedBadges(uid);
			return model;
		}

		public async Task<PersonModel> RemovePersonToken(Guid uid)
		{
			var entity = await _personRepository.GetPerson(uid);
			var model = _mapper.Map<PersonModel>(entity);
			entity.PersonImageContentEntity = null;
			entity.FriendList = null;
			entity.City = null;
			entity.SwipeHistory = null;
			entity.Token = null;
			await _personRepository.UpdatePerson(entity);
			return model;
		}

		public async Task AddFeedback(FeedbackModel model, Guid uid)
		{
			var entity = _mapper.Map<FeedbackEntity>(model);
			var personEntity = await _personRepository.GetPerson(uid);
			entity.PersonId = personEntity.PersonId;
			entity.FeedbackTime = DateTime.UtcNow;
			entity.FeedbackUid = Guid.NewGuid();
			if (model.Images != null)
			{
				var images = new List<FeedbackImageContentEntity>();
				foreach (var image in model.Images)
				{
					var imageUid = await _imageLogic.SaveImage(image);
					images.Add(new FeedbackImageContentEntity { FeedbackImageContentUid = imageUid });
				}
				entity.FeedbackImageContentEntities = images;
			}
			await _personRepository.AddFeedback(entity);
		}

		public async Task<List<PersonModel>> GetPersonList(List<Guid> personUids, Guid personUid)
		{
			var personEntities = await _personRepository.GetPersonList(personUids);
			var personModels = _mapper.Map<List<PersonModel>>(personEntities.Where(x => !string.IsNullOrEmpty(x.Login)));
			var personFriends = await _personRepository.GetAllPersonFriends(personUid);
			var notApprovedFriends = await _personRepository.GetNewFriends(personUid);
			var currentPersonModel = personModels.SingleOrDefault(x => x.PersonUid == personUid);
			if (currentPersonModel != null)
			{
				personModels.Remove(currentPersonModel);
			}
			foreach (var model in personModels)
			{
				model.IsFriend = personFriends.Any(x => x.PersonUid == model.PersonUid);
				model.FriendshipApprovalRequired = notApprovedFriends.Any(x => x.PersonUid == model.PersonUid);
			}

			return personModels;
		}

		public async Task AddReport(PersonReportModel model, Guid uid)
		{
			var personEntity = await _personRepository.GetPerson(model.PersonUid);
			var authorEntity = await _personRepository.GetPerson(uid);
			var reportEntity = new PersonReportEntity();
			reportEntity.Text = model.Text;
			reportEntity.PersonId = personEntity.PersonId;
			reportEntity.AuthorId = authorEntity.PersonId;
			reportEntity.CreationTime = DateTime.UtcNow;
			reportEntity.IsProcessed = false;
			reportEntity.PersonReportUid = Guid.NewGuid();
			await _personRepository.AddReport(reportEntity);
		}
	}
}
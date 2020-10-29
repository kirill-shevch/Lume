using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Event;
using BLL.Notification.Interfaces;
using Constants;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using DAL.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class EventLogic : IEventLogic
	{
		private readonly IEventRepository _eventRepository;
		private readonly IPersonRepository _personRepository;
		private readonly IImageLogic _imageLogic;
		private readonly IMapper _mapper;
		private readonly IPushNotificationService _pushNotificationService;

		public EventLogic(IEventRepository eventRepository,
			IPersonRepository personRepository,
			IMapper mapper,
			IPushNotificationService pushNotificationService,
			IImageLogic imageLogic)
		{
			_eventRepository = eventRepository;
			_personRepository = personRepository;
			_mapper = mapper;
			_pushNotificationService = pushNotificationService;
			_imageLogic = imageLogic;
		}

		public async Task<Guid> AddEvent(AddEventModel addEventModel, Guid personUid)
		{
			var eventUid = Guid.NewGuid();
			var person = await _personRepository.GetPerson(personUid);
			var entity = _mapper.Map<EventEntity>(addEventModel);
			entity.EventUid = eventUid;
			entity.AdministratorId = person.PersonId;
			entity.Chat = new ChatEntity { ChatUid = Guid.NewGuid(), IsGroupChat = true };
			var imageList = new List<EventImageContentEntity>();
			if (addEventModel.PrimaryImage != null && addEventModel.PrimaryImage.Any())
			{
				var eventImageContentUid = await _imageLogic.SaveImage(addEventModel.PrimaryImage);
				imageList.Add(new EventImageContentEntity 
				{ 
					EventImageContentUid = eventImageContentUid, 
					IsPrimary = true 
				});
			}
			if (addEventModel.Images != null)
			{
				foreach (var image in addEventModel.Images)
				{
					var eventImageContentUid = await _imageLogic.SaveImage(image);
					imageList.Add(new EventImageContentEntity
					{
						EventImageContentUid = eventImageContentUid,
						IsPrimary = false
					});
				}
			}
			entity.EventImageContentEntities = imageList;
			await _eventRepository.CreateEvent(entity);
			await AddParticipant(new EventParticipantModel { EventUid = eventUid, PersonUid = person.PersonUid, ParticipantStatus = ParticipantStatus.Active }, personUid);
			return eventUid;
		}

		public async Task<GetEventModel> GetEvent(Guid eventUid)
		{
			var eventEntity = await _eventRepository.GetEvent(eventUid);
			var eventModel = _mapper.Map<GetEventModel>(eventEntity);
			eventModel.PromoRequestUid = eventEntity.PromoRewardRequests == null ? null : eventEntity.PromoRewardRequests.FirstOrDefault()?.PromoRewardRequestUid;
			foreach (var participant in eventModel.Participants)
			{
				var status = eventEntity.Participants.Single(x => x.Person.PersonUid == participant.PersonUid).ParticipantStatusId;
				participant.ParticipantStatus = (ParticipantStatus)status;
			}
			return eventModel;
		}

		public async Task<List<GetEventListModel>> GetEventList(Guid personUid)
		{
			var eventEntities = await _eventRepository.GetEvents(personUid);
			return eventEntities.Select(entity => 
			{
				var model = _mapper.Map<GetEventListModel>(entity);
				model.IsAdministrator = entity.Administrator.PersonUid == personUid;
				var status = entity.Participants.Single(s => s.Person.PersonUid == personUid).ParticipantStatusId;
				model.ParticipantStatus = (ParticipantStatus)status;
				model.AnyPersonWaitingForApprove = model.IsAdministrator.Value && entity.Participants.Any(x => x.ParticipantStatusId == (long)ParticipantStatus.WaitingForApproveFromEvent);
				return model;
			}).Where(x => x.ParticipantStatus != ParticipantStatus.Rejected).ToList();
		}
		public async Task<GetEventModel> UpdateEvent(UpdateEventModel updateEventModel)
		{
			var eventEntity = await _eventRepository.GetEvent(updateEventModel.EventUid);
			if (!string.IsNullOrEmpty(updateEventModel.Name))
				eventEntity.Name = updateEventModel.Name;
			if (updateEventModel.MinAge.HasValue)
				eventEntity.MinAge = updateEventModel.MinAge;
			if (updateEventModel.MaxAge.HasValue)
				eventEntity.MaxAge = updateEventModel.MaxAge;
			if (updateEventModel.XCoordinate.HasValue)
				eventEntity.XCoordinate = updateEventModel.XCoordinate.Value;
			if (updateEventModel.YCoordinate.HasValue)
				eventEntity.YCoordinate = updateEventModel.YCoordinate.Value;
			if (!string.IsNullOrEmpty(updateEventModel.Description))
				eventEntity.Description = updateEventModel.Description;
			if (updateEventModel.StartTime.HasValue)
				eventEntity.StartTime = updateEventModel.StartTime;
			if (updateEventModel.EndTime.HasValue)
				eventEntity.EndTime = updateEventModel.EndTime;
			if (updateEventModel.IsOpenForInvitations.HasValue)
				eventEntity.IsOpenForInvitations = updateEventModel.IsOpenForInvitations;
			if (updateEventModel.IsOnline.HasValue)
				eventEntity.IsOnline = updateEventModel.IsOnline;
			if (updateEventModel.CityId.HasValue)
				eventEntity.CityId = updateEventModel.CityId;

			var images = new List<EventImageContentEntity>();
			if (updateEventModel.ExtraImages != null && updateEventModel.ExtraImages.Any())
			{
				foreach (var image in updateEventModel.ExtraImages)
				{
					var eventImageContentUid = await _imageLogic.SaveImage(image);
					images.Add(new EventImageContentEntity
					{
						EventImageContentUid = eventImageContentUid,
						IsPrimary = false
					});
				}
				eventEntity.EventImageContentEntities = images;
			}
			if (updateEventModel.PrimaryImage != null && updateEventModel.PrimaryImage.Any())
			{
				var eventImageContentUid = await _imageLogic.SaveImage(updateEventModel.PrimaryImage);
				images.Add(new EventImageContentEntity
				{
					EventImageContentUid = eventImageContentUid,
					IsPrimary = true
				});
			}
			eventEntity.EventImageContentEntities = images;

			if (updateEventModel.Types != null && updateEventModel.Types.Any())
			{
				await _eventRepository.RemoveEventTypes(eventEntity.EventId);
				eventEntity.EventTypes = new List<EventTypeToEventEntity>(
					updateEventModel.Types.Select(x => new EventTypeToEventEntity { EventId = eventEntity.EventId, EventTypeId = (long)x }));
			}
			else
			{
				eventEntity.EventTypes = null;
			}
			if (updateEventModel.Status.HasValue)
				eventEntity.EventStatusId = (long)updateEventModel.Status;
			eventEntity.EventStatus = null;
			eventEntity.City = null;
			eventEntity.Administrator = null;
			eventEntity.Participants = null;
			eventEntity.Chat = null;
			eventEntity.SwipeHistory = null;
			await _eventRepository.UpdateEvent(eventEntity);
			return await GetEvent(eventEntity.EventUid);
		}

		public async Task<GetEventModel> AddParticipant(EventParticipantModel eventParticipantModel, Guid personUid)
		{
			var entity = await CreateParticipantEntity(eventParticipantModel);
			await _eventRepository.AddParticipant(entity);
			var person = await _personRepository.GetPerson(eventParticipantModel.PersonUid);
			var eventEntity = await _eventRepository.GetEvent(eventParticipantModel.EventUid);
			if (!string.IsNullOrEmpty(person.Token) && eventParticipantModel.PersonUid != personUid)
			{
				await _pushNotificationService.SendPushNotification(person.Token, 
					MessageTitles.AddParticipantNotificationMessage, 
					new Dictionary<FirebaseNotificationKeys, string> { [FirebaseNotificationKeys.Url] = string.Format(FirebaseNotificationTemplates.EventUrlTemplate, eventEntity.EventUid) }, 
					eventEntity.Name);
			}
			if (eventEntity.Administrator != null && eventEntity.Administrator.Token != null && eventEntity.Administrator.PersonUid != personUid)
			{
				if (eventParticipantModel.ParticipantStatus == ParticipantStatus.WaitingForApproveFromEvent)
				{
					await _pushNotificationService.SendPushNotification(eventEntity.Administrator.Token,
						MessageTitles.ParticipantWaitingForApproval,
						new Dictionary<FirebaseNotificationKeys, string> { [FirebaseNotificationKeys.Url] = string.Format(FirebaseNotificationTemplates.EventUrlTemplate, eventEntity.EventUid) },
						eventEntity.Name);
				}
				else if (eventParticipantModel.ParticipantStatus == ParticipantStatus.Active)
				{
					await _pushNotificationService.SendPushNotification(eventEntity.Administrator.Token,
						MessageTitles.ParticipantJoinedTheEvent,
						new Dictionary<FirebaseNotificationKeys, string> { [FirebaseNotificationKeys.Url] = string.Format(FirebaseNotificationTemplates.EventUrlTemplate, eventEntity.EventUid) },
						eventEntity.Name);
				}
			}
			return await GetEvent(eventParticipantModel.EventUid);
		}

		public async Task UpdateParticipant(EventParticipantModel eventParticipantModel)
		{
			var entity = await CreateParticipantEntity(eventParticipantModel);
			await _eventRepository.UpdateParticipant(entity);
		}

		public async Task RemoveParticipant(Guid personUid, Guid eventUid)
		{
			var entity = await _eventRepository.GetParticipant(personUid, eventUid);
			await _eventRepository.RemoveParticipant(entity);
		}

		private async Task<PersonToEventEntity> CreateParticipantEntity(EventParticipantModel eventParticipantModel)
		{
			var personEntity = await _personRepository.GetPerson(eventParticipantModel.PersonUid);
			var eventEntity = await _eventRepository.GetEvent(eventParticipantModel.EventUid);
			return new PersonToEventEntity
			{
				EventId = eventEntity.EventId,
				PersonId = personEntity.PersonId,
				ParticipantStatusId = (long)eventParticipantModel.ParticipantStatus
			};
		}

		public async Task<GetEventModel> GetRandomEvent(RandomEventFilter filter, Guid personUid)
		{
			var repositoryFilter = _mapper.Map<RepositoryRandomEventFilter>(filter);
			var personEntity = await _personRepository.GetPerson(personUid);
			repositoryFilter.Age = personEntity.Age.Value;
			repositoryFilter.PersonUid = personUid;
			repositoryFilter.IgnoringEventList = personEntity.SwipeHistory.Select(x => x.EventId).ToList();
			var entity = await _eventRepository.GetRandomEvent(repositoryFilter);
			return _mapper.Map<GetEventModel>(entity);
		}

		public async Task<List<GetEventListModel>> SearchForEvent(EventSearchFilter filter)
		{
			var repositoryFilter = _mapper.Map<RepositoryEventSearchFilter>(filter);
			var entities = await _eventRepository.SearchForEvent(repositoryFilter);
			return _mapper.Map<List<GetEventListModel>>(entities);
		}

		public async Task AddEventSwipeHistory(Guid personUid, Guid eventUid)
		{
			var eventEntity = await _eventRepository.GetEvent(eventUid);
			var personEntity = await _personRepository.GetPerson(personUid);
			await _eventRepository.AddEventSwipeHistoryRecord(new EventSwipeHistoryEntity { EventId = eventEntity.EventId, PersonId = personEntity.PersonId });
		}

		public async Task AddPromoRewardRequest(PromoRewardRequestModel request)
		{
			var eventEntity = await _eventRepository.GetEvent(request.EventUid);
			var entity = new PromoRewardRequestEntity();
			entity.AccountingNumber = request.AccountingNumber;
			entity.PromoRewardRequestUid = Guid.NewGuid();
			entity.PromoRewardRequestTime = DateTime.UtcNow;
			entity.EventId = eventEntity.EventId;
			if (request.Images != null)
			{
				var images = new List<PromoRewardRequestImageContentEntity>();
				foreach (var image in request.Images)
				{
					var imageUid = await _imageLogic.SaveImage(image);
					images.Add(new PromoRewardRequestImageContentEntity { PromoRewardRequestImageContentUid = imageUid });
				}
				entity.Images = images;
			}
			await _eventRepository.AddPromoRewardRequest(entity);
		}

		public async Task RemoveEventImage(RemoveEventImageModel request)
		{
			var eventEntity = await _eventRepository.GetEvent(request.EventUid);
			var imageEntity = eventEntity.EventImageContentEntities.Single(x => x.EventImageContentUid == request.ImageUid);
			await _eventRepository.RemoveEventImage(imageEntity);
			await _imageLogic.RemoveImage(request.ImageUid);
		}

		public async Task AddReport(EventReportModel model, Guid uid)
		{
			var eventEntity = await _eventRepository.GetEvent(model.EventUid);
			var authorEntity = await _personRepository.GetPerson(uid);
			var reportEntity = new EventReportEntity();
			reportEntity.Text = model.Text;
			reportEntity.EventId = eventEntity.EventId;
			reportEntity.AuthorId = authorEntity.PersonId;
			reportEntity.CreationTime = DateTime.UtcNow;
			reportEntity.IsProcessed = false;
			reportEntity.EventReportUid = Guid.NewGuid();
			await _eventRepository.AddReport(reportEntity);
		}
	}
}

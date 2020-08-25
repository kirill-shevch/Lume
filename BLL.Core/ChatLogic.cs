using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Chat;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class ChatLogic : IChatLogic
	{
		private readonly IChatRepository _chatRepository;
		private readonly IEventRepository _eventRepository;
		private readonly IImageLogic _imageLogic;
		private readonly IPersonRepository _personRepository;
		private readonly IMapper _mapper;
		public ChatLogic(IChatRepository chatRepository,
			IEventRepository eventRepository,
			IImageLogic imageLogic,
			IPersonRepository personRepository,
			IMapper mapper)
		{
			_chatRepository = chatRepository;
			_eventRepository = eventRepository;
			_imageLogic = imageLogic;
			_personRepository = personRepository;
			_mapper = mapper;
		}

		public async Task<ChatMessageModel> AddChatMessage(AddMessageModel request, Guid personUid)
		{
			var chatEntity = await _chatRepository.GetChat(request.ChatUid);
			var personEntity = await _personRepository.GetPerson(personUid);
			var chatMessageUid = Guid.NewGuid();
			var date = DateTime.UtcNow;
			var messageEntity = new ChatMessageEntity
			{
				ChatMessageUid = chatMessageUid,
				Content = request.Content,
				MessageTime = date,
				ChatId = chatEntity.ChatId,
				AuthorId = personEntity.PersonId
			};
			await _chatRepository.AddChatMessage(messageEntity);
			var chatImageUids = new List<Guid>();
			foreach (var image in request.Images)
			{
				var chatImageUid = await _imageLogic.SaveImage(image);
				await _chatRepository.SaveChatImage(chatMessageUid, new ChatImageContentEntity { ChatImageContentUid = chatImageUid });
				chatImageUids.Add(chatImageUid);
			}
			await _chatRepository.AddLastReadChatMessage(chatEntity, personUid, messageEntity.ChatMessageId);
			return new ChatMessageModel 
			{ 
				Images = chatImageUids,
				MessageContent = request.Content,
				MessageUid = chatMessageUid,
				PersonUid = personEntity.PersonUid,
				PersonName = personEntity.Name,
				MessageTime = date,
				PersonImageUid = personEntity.PersonImageContentEntity?.PersonImageContentUid
			};
		}

		public async Task<ChatModel> GetChat(Guid chatUid, int pageNumber, int pageSize, Guid personUid)
		{
			var chatEntity = await _chatRepository.GetChat(chatUid);
			var unreadMessagesCount = await _chatRepository.GetChatUnreadMessagesCount(chatEntity, personUid);
			var chatModel = _mapper.Map<ChatModel>(chatEntity);
			var chatMessageEntities = await _chatRepository.GetChatMessages(chatEntity.ChatId, pageNumber, pageSize);
			chatModel.Messages = _mapper.Map<List<ChatMessageModel>>(chatMessageEntities);
			if (chatEntity.IsGroupChat.HasValue && chatEntity.IsGroupChat.Value)
			{
				var eventEntity = await _eventRepository.GetEventByChatId(chatEntity.ChatId);
				chatModel.ChatName = eventEntity.Name;
				chatModel.EventUid = eventEntity.EventUid;
				chatModel.EventImageUid = eventEntity.EventImageContentEntities.SingleOrDefault(x => x.IsPrimary.HasValue && x.IsPrimary.Value)?.EventImageContentUid;
			}
			else if (chatEntity.IsGroupChat.HasValue && !chatEntity.IsGroupChat.Value)
			{
				var personEntity = await _personRepository.GetPerson(personUid);
				chatModel.ChatName = await _chatRepository.GetPersonalChatName(chatEntity.ChatId, personEntity.PersonId);
				chatModel.PersonUid = personEntity.PersonUid;
				chatModel.PersonImageUid = personEntity.PersonImageContentEntity?.PersonImageContentUid;
			}
			if (chatMessageEntities.Any())
			{
				await _chatRepository.AddLastReadChatMessage(chatEntity, personUid, chatMessageEntities.Max(x => x.ChatMessageId));
			}
			chatModel.UnreadMessagesCount = unreadMessagesCount;
			return chatModel;
		}

		public async Task<List<ChatMessageModel>> GetNewChatMessages(Guid chatUid, Guid? messageUid, Guid personUid)
		{
			var numberOfAttempts = 30;
			var waitingInterval = 3000;
			var chatEntity = await _chatRepository.GetChat(chatUid);
			var personEntity = await _personRepository.GetPerson(personUid);
			long chatMessageId = 0;
			if (messageUid.HasValue)
			{
				var chatMessageEntity = await _chatRepository.GetChatMessage(messageUid.Value);
				chatMessageId = chatMessageEntity.ChatMessageId;
			}
			for (int i = 0; i < numberOfAttempts; i++)
			{
				var messageEntities = await _chatRepository.GetNewChatMessages(chatEntity.ChatId, chatMessageId, personEntity.PersonId);
				if (messageEntities != null && messageEntities.Any())
				{
					await _chatRepository.AddLastReadChatMessage(chatEntity, personUid, messageEntities.Max(x => x.ChatMessageId));
					return _mapper.Map<List<ChatMessageModel>>(messageEntities);
				}
				Thread.Sleep(waitingInterval);
			}
			return new List<ChatMessageModel>();
		}

		public async Task<ChatModel> GetPersonChat(Guid uid, Guid personUid, int pageSize)
		{
			var chatEntity = await _chatRepository.GetPersonChat(uid, personUid);
			var unreadMessagesCount = await _chatRepository.GetChatUnreadMessagesCount(chatEntity, uid);
			var personEntity = await _personRepository.GetPerson(personUid);
			if (chatEntity == null)
			{
				return new ChatModel 
				{ 
					ChatUid = await _chatRepository.CreatePersonalChat(uid, personUid), 
					IsGroupChat = false, 
					ChatName = personEntity.Name,
					PersonUid = personEntity.PersonUid,
					PersonImageUid = personEntity.PersonImageContentEntity?.PersonImageContentUid
			};
			}
			var chatModel = _mapper.Map<ChatModel>(chatEntity);
			chatModel.ChatName = personEntity.Name;
			chatModel.PersonUid = personEntity.PersonUid;
			chatModel.PersonImageUid = personEntity.PersonImageContentEntity?.PersonImageContentUid;
			var chatMessageEntities = await _chatRepository.GetChatMessages(chatEntity.ChatId, 1, pageSize);
			if (chatMessageEntities.Any())
			{
				await _chatRepository.AddLastReadChatMessage(chatEntity, uid, chatMessageEntities.Max(x => x.ChatMessageId));
			}
			chatModel.Messages = _mapper.Map<List<ChatMessageModel>>(chatMessageEntities);
			chatModel.UnreadMessagesCount = unreadMessagesCount;
			return chatModel;
		}

		public async Task<List<ChatListModel>> GetPersonChatList(Guid uid)
		{
			var events = await _eventRepository.GetEvents(uid);
			var chatModels = new List<ChatListModel>();
			foreach (var eventEntity in events)
			{
				var groupChatModel = _mapper.Map<ChatListModel>(eventEntity.Chat);
				groupChatModel.Name = eventEntity.Name;
				var lastGroupChatMessageEntity = await _chatRepository.GetChatMessages(eventEntity.ChatId, 1, 1);
				groupChatModel.LastMessage = _mapper.Map<ChatMessageModel>(lastGroupChatMessageEntity.SingleOrDefault());
				groupChatModel.EventImageUid = eventEntity.EventImageContentEntities.SingleOrDefault(x => x.IsPrimary.HasValue && x.IsPrimary.Value)?.EventImageContentUid;
				groupChatModel.UnreadMessagesCount = await _chatRepository.GetChatUnreadMessagesCount(eventEntity.Chat, uid);
				chatModels.Add(groupChatModel);
			}
			var personToChatEntities = await _chatRepository.GetPersonChats(uid);
			foreach (var entity in personToChatEntities)
			{
				var personalChatModel = _mapper.Map<ChatListModel>(entity.Chat);
				var personEntity = entity.FirstPerson.PersonUid == uid ? entity.SecondPerson : entity.FirstPerson;
				personalChatModel.Name = personEntity.Name;
				personalChatModel.PersonImageUid = personEntity.PersonImageContentEntity?.PersonImageContentUid;
				var lastPersonalChatMessageEntity = await _chatRepository.GetChatMessages(entity.ChatId, 1, 1);
				personalChatModel.LastMessage = _mapper.Map<ChatMessageModel>(lastPersonalChatMessageEntity.SingleOrDefault());
				personalChatModel.UnreadMessagesCount = await _chatRepository.GetChatUnreadMessagesCount(entity.Chat, uid);
				chatModels.Add(personalChatModel);
			}
			return chatModels.OrderByDescending(x => x.LastMessage?.MessageTime).ToList();
		}
	}
}

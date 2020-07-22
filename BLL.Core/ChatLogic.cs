using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Chat;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class ChatLogic : IChatLogic
	{
		private readonly IChatRepository _chatRepository;
		private readonly IEventRepository _eventRepository;
		private readonly IImageRepository _imageRepository;
		private readonly IPersonRepository _personRepository;
		private readonly IMapper _mapper;
		public ChatLogic(IChatRepository chatRepository,
			IEventRepository eventRepository,
			IImageRepository imageRepository,
			IPersonRepository personRepository,
			IMapper mapper)
		{
			_chatRepository = chatRepository;
			_eventRepository = eventRepository;
			_imageRepository = imageRepository;
			_personRepository = personRepository;
			_mapper = mapper;
		}

		public async Task<ChatMessageModel> AddChatMessage(AddMessageModel request, Guid personUid)
		{
			var chatEntity = await _chatRepository.GetChat(request.ChatUid);
			var personEntity = await _personRepository.GetPerson(personUid);
			var chatMessageUid = Guid.NewGuid();
			await _chatRepository.AddChatMessage(new ChatMessageEntity {
				ChatMessageUid = chatMessageUid,
				Content = request.Content,
				MessageTime = DateTime.UtcNow,
				ChatId = chatEntity.ChatId,
				AuthorId = personEntity.PersonId
			});
			var chatImageUids = new List<Guid>();
			foreach (var image in request.Images)
			{
				var chatImageUid = Guid.NewGuid();
				await _imageRepository.SaveChatImage(chatMessageUid, new ChatImageContentEntity { Content = image, ChatImageContentUid = chatMessageUid });
				chatImageUids.Add(chatImageUid);
			}
			return new ChatMessageModel 
			{ 
				Images = chatImageUids,
				MessageContent = request.Content,
				MessageUid = chatMessageUid,
				PersonUid = personEntity.PersonUid,
				PersonName = personEntity.Name,
				PersonImageUid = personEntity.PersonImageContentEntity?.PersonImageContentUid
			};
		}

		public async Task<ChatModel> GetChat(Guid chatUid, int pageNumber, int pageSize, Guid personUid)
		{
			var chatEntity = await _chatRepository.GetChat(chatUid);
			var chatModel = _mapper.Map<ChatModel>(chatEntity);
			var chatMessageEntities = await _chatRepository.GetChatMessages(chatEntity.ChatId, pageNumber, pageSize);
			chatModel.Messages = _mapper.Map<List<ChatMessageModel>>(chatMessageEntities);
			if (chatEntity.IsGroupChat.HasValue && chatEntity.IsGroupChat.Value)
			{
				chatModel.ChatName = await _eventRepository.GetEventNameByChatId(chatEntity.ChatId);
			}
			else if (chatEntity.IsGroupChat.HasValue && !chatEntity.IsGroupChat.Value)
			{
				var personEntity = await _personRepository.GetPerson(personUid);
				chatModel.ChatName = await _chatRepository.GetPersonalChatName(chatEntity.ChatId, personEntity.PersonId);
			}
			return chatModel;
		}

		public async Task<ChatModel> GetPersonChat(Guid uid, Guid personUid, int pageSize)
		{
			var chatEntity = await _chatRepository.GetPersonChat(uid, personUid);
			var personEntity = await _personRepository.GetPerson(personUid);
			if (chatEntity == null)
			{
				return new ChatModel { ChatUid = await _chatRepository.CreatePersonalChat(uid, personUid), IsGroupChat = false, ChatName = personEntity.Name };
			}
			var chatModel = _mapper.Map<ChatModel>(chatEntity);
			chatModel.ChatName = personEntity.Name;
			var chatMessageEntities = await _chatRepository.GetChatMessages(chatEntity.ChatId, 1, pageSize);
			chatModel.Messages = _mapper.Map<List<ChatMessageModel>>(chatMessageEntities);
			return chatModel;
		}

		public async Task<List<ChatListModel>> GetPersonChatList(Guid uid)
		{
			var events = await _eventRepository.GetEvents(uid);
			var groupChats = events.Select(x => x.Chat).ToList();
			var personalChats = await _chatRepository.GetPersonChats(uid);
			var chats = groupChats.Union(personalChats);
			var chatModels = _mapper.Map<IEnumerable<ChatListModel>>(chats);
			return chatModels.ToList();
		}
	}
}

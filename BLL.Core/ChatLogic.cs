using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Chat;
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
		private readonly IMapper _mapper;
		public ChatLogic(IChatRepository chatRepository,
			IEventRepository eventRepository,
			IMapper mapper)
		{
			_chatRepository = chatRepository;
			_eventRepository = eventRepository;
			_mapper = mapper;
		}

		public async Task<ChatModel> GetChat(Guid chatUid)
		{
			var chatEntity = await _chatRepository.GetChat(chatUid);
			var chatModel = _mapper.Map<ChatModel>(chatEntity);
			chatModel.Messages = new List<ChatMessageModel>();
			return chatModel;
		}

		public async Task<ChatModel> GetPersonChat(Guid uid, Guid personUid)
		{
			var chatEntity = await _chatRepository.GetPersonChat(uid, personUid);
			if (chatEntity == null)
			{
				return new ChatModel { ChatUid = await _chatRepository.CreatePersonalChat(uid, personUid), IsGroupChat = false };
			}
			var chatModel = _mapper.Map<ChatModel>(chatEntity);
			chatModel.Messages = new List<ChatMessageModel>();
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

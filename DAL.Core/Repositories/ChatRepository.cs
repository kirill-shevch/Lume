using DAL.Core.Entities;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Core.Repositories
{
	public class ChatRepository : IChatRepository
	{
		private readonly ICoreContextFactory _dbContextFactory;
		public ChatRepository(ICoreContextFactory dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		public async Task<ChatEntity> GetChat(Guid uid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.ChatEntities.SingleOrDefaultAsync(x => x.ChatUid == uid, cancellationToken);
			}
		}

		public async Task<List<ChatMessageEntity>> GetChatMessages(long chatId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.ChatMessageEntities
					.Include(x => x.ChatImageContentEntities)
					.Include(x => x.Author)
						.ThenInclude(x => x.PersonImageContentEntity)
					.Where(x => x.ChatId == chatId)
					.OrderByDescending(x => x.ChatMessageId)
					.Skip(pageSize * (pageNumber - 1))
					.Take(pageSize)
					.OrderBy(x => x.ChatMessageId)
					.ToListAsync(cancellationToken);
			}
		}

		public async Task<bool> CheckChatMessageExistence(Guid chatMessageUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.ChatMessageEntities.AnyAsync(x => x.ChatMessageUid == chatMessageUid, cancellationToken);
			}
		}

		public async Task<bool> CheckChatExistence(Guid chatUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.ChatEntities.AnyAsync(x => x.ChatUid == chatUid, cancellationToken);
			}
		}

		public async Task<ChatEntity> GetPersonChat(Guid userUid, Guid friendUid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var personToChat = await context.PersonToChatEntities
					.Include(x => x.FirstPerson)
					.Include(x => x.SecondPerson)
					.Include(x => x.Chat)
						.Where(x => x.FirstPerson.PersonUid == userUid && x.SecondPerson.PersonUid == friendUid)
					.SingleOrDefaultAsync();
				return personToChat?.Chat;
			}
		}

		public async Task<string> GetPersonalChatName(long chatId, long personId)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var personToChat = await context.PersonToChatEntities
					.Include(x => x.FirstPerson)
					.Include(x => x.SecondPerson)
					.SingleOrDefaultAsync(x => x.ChatId == chatId && x.FirstPersonId == personId);
				if (personToChat == null)
				{
					return string.Empty;
				}
				return personToChat.FirstPersonId == personId ? personToChat.SecondPerson.Name : personToChat.FirstPerson.Name;
			}
		}

		public async Task<Guid> CreatePersonalChat(Guid firstPersonUid, Guid secondPersonUid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var chatUid = Guid.NewGuid();
				var firstPerson = await context.PersonEntities.SingleAsync(x => x.PersonUid == firstPersonUid);
				var secondPerson = await context.PersonEntities.SingleAsync(x => x.PersonUid == secondPersonUid);
				var chat = new ChatEntity { ChatUid = chatUid, IsGroupChat = false };
				await context.ChatEntities.AddAsync(chat);
				await context.PersonToChatEntities.AddAsync(new PersonToChatEntity { FirstPerson = firstPerson, SecondPerson = secondPerson, Chat = chat });
				await context.PersonToChatEntities.AddAsync(new PersonToChatEntity { FirstPerson = secondPerson, SecondPerson = firstPerson, Chat = chat });
				await context.SaveChangesAsync();
				return chatUid;
			}
		}

		public async Task<List<PersonToChatEntity>> GetPersonChats(Guid uid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var entities = await context.PersonToChatEntities
					.Include(x => x.FirstPerson)
						.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.SecondPerson)
						.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.Chat)
					.Where(x => x.FirstPerson.PersonUid == uid)
					.ToListAsync();
				return entities;

			}
		}

		public async Task AddChatMessage(ChatMessageEntity chatMessageEntity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				await context.AddAsync(chatMessageEntity);
				await context.SaveChangesAsync();
			}
		}

		public async Task<ChatMessageEntity> GetChatMessage(Guid uid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.ChatMessageEntities.SingleAsync(x => x.ChatMessageUid == uid);
			}
		}

		public async Task<List<ChatMessageEntity>> GetNewChatMessages(long chatId, long lastMessageId, long personId)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.ChatMessageEntities.Where(x => x.ChatId == chatId && x.ChatMessageId > lastMessageId && x.AuthorId != personId)
					.Include(x => x.Author)
						.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.ChatImageContentEntities)
					.OrderBy(x => x.ChatMessageId)
					.ToListAsync();
			}
		}

		public async Task SaveChatImage(Guid chatMessageUid, ChatImageContentEntity entity)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var chatMessageEntity = await context.ChatMessageEntities.SingleAsync(x => x.ChatMessageUid == chatMessageUid);
				entity.ChatMessageId = chatMessageEntity.ChatMessageId;
				await context.AddAsync(entity);
				await context.SaveChangesAsync();
			}
		}

		public async Task AddLastReadChatMessage(ChatEntity chatEntity, Guid personUid, long messageId)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				if (chatEntity.IsGroupChat.HasValue && chatEntity.IsGroupChat.Value)
				{
					var entity = await context.PersonToEventEntities
						.Include(x => x.Event)
						.Include(x => x.Person)
						.SingleOrDefaultAsync(x => x.Person.PersonUid == personUid && x.Event.ChatId == chatEntity.ChatId);
					if (!entity.LastReadChatMessageId.HasValue || entity.LastReadChatMessageId < messageId)
					{
						entity.LastReadChatMessageId = messageId;
						context.PersonToEventEntities.Update(entity);
						await context.SaveChangesAsync();
					}
				}
				else if (chatEntity.IsGroupChat.HasValue && !chatEntity.IsGroupChat.Value)
				{
					var entity = await context.PersonToChatEntities
						.Include(x => x.FirstPerson)
						.SingleOrDefaultAsync(x => x.ChatId == chatEntity.ChatId && x.FirstPerson.PersonUid == personUid);
					if (!entity.LastReadChatMessageId.HasValue || entity.LastReadChatMessageId < messageId)
					{
						entity.LastReadChatMessageId = messageId;
						context.PersonToChatEntities.Update(entity);
						await context.SaveChangesAsync();
					}
				}
			}
		}

		public async Task<bool> CheckPersonForNewChatMessages(Guid personUid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var personEvents = await context.PersonToEventEntities
					.Include(x => x.Person)
					.Include(x => x.Event)
						.ThenInclude(x => x.Chat)
							.ThenInclude(x => x.ChatMessageEntities)
					.AnyAsync(x => ((!x.LastReadChatMessageId.HasValue && x.Event.Chat.ChatMessageEntities.Any()) || (x.LastReadChatMessageId < x.Event.Chat.ChatMessageEntities.Max(x => x.ChatMessageId))) 
						&& x.Person.PersonUid == personUid);
				var personalChats = await context.PersonToChatEntities
					.Include(x => x.FirstPerson)
					.Include(x => x.Chat)
						.ThenInclude(x => x.ChatMessageEntities)
					.AnyAsync(x => ((!x.LastReadChatMessageId.HasValue && x.Chat.ChatMessageEntities.Any()) || (x.LastReadChatMessageId < x.Chat.ChatMessageEntities.Max(x => x.ChatMessageId)))
						&& x.FirstPerson.PersonUid == personUid);
				return personalChats || personEvents;
				
			}
		}

		public async Task<int> GetChatUnreadMessagesCount(ChatEntity chat, Guid uid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				if (chat == null)
					return 0;
				if (chat.IsGroupChat.HasValue && chat.IsGroupChat.Value)
				{
					var personToEventEntity = await context.PersonToEventEntities
						.Include(x => x.Person)
						.Include(x => x.Event)
						.SingleAsync(x => x.Event.ChatId == chat.ChatId && x.Person.PersonUid == uid);
					if (!personToEventEntity.LastReadChatMessageId.HasValue)
					{
						return await context.ChatMessageEntities.Where(x => x.ChatId == chat.ChatId).CountAsync();
					}
					return await context.ChatMessageEntities.Where(x => x.ChatId == chat.ChatId).CountAsync(x => x.ChatMessageId > personToEventEntity.LastReadChatMessageId);
				}
				else if (chat.IsGroupChat.HasValue && !chat.IsGroupChat.Value)
				{
					var personToChatEntity = await context.PersonToChatEntities
					.Include(x => x.FirstPerson)
					.SingleAsync(x => x.ChatId == chat.ChatId && x.FirstPerson.PersonUid == uid);
					if (!personToChatEntity.LastReadChatMessageId.HasValue)
					{
						return await context.ChatMessageEntities.Where(x => x.ChatId == chat.ChatId).CountAsync();
					}
					return await context.ChatMessageEntities.Where(x => x.ChatId == chat.ChatId).CountAsync(x => x.ChatMessageId > personToChatEntity.LastReadChatMessageId);
				}
				return 0;
			}
		}
	}
}

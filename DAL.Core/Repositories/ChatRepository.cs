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
		private readonly CoreContextFactory _dbContextFactory;
		public ChatRepository(CoreContextFactory dbContextFactory)
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
						.Where(x => (x.FirstPerson.PersonUid == userUid || x.FirstPerson.PersonUid == friendUid) &&
						(x.SecondPerson.PersonUid == userUid || x.SecondPerson.PersonUid == friendUid))
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
					.SingleOrDefaultAsync(x => x.ChatId == chatId && (x.FirstPersonId == personId || x.SecondPersonId == personId));
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
					.Where(x => x.FirstPerson.PersonUid == uid || x.SecondPerson.PersonUid == uid)
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

		public async Task<List<ChatMessageEntity>> GetNewChatMessages(long chatId, long lastMessageId)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.ChatMessageEntities.Where(x => x.ChatId == chatId && x.ChatMessageId > lastMessageId)
					.Include(x => x.Author)
						.ThenInclude(x => x.PersonImageContentEntity)
					.Include(x => x.ChatImageContentEntities)
					.OrderByDescending(x => x.ChatMessageId)
					.ToListAsync();
			}
		}
	}
}

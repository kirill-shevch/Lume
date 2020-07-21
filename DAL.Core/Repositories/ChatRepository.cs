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

		public async Task<List<ChatEntity>> GetPersonChats(Guid uid)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				var chats = await context.PersonToChatEntities
					.Include(x => x.FirstPerson)
					.Include(x => x.SecondPerson)
					.Include(x => x.Chat)
					.Where(x => x.FirstPerson.PersonUid == uid || x.SecondPerson.PersonUid == uid)
					.ToListAsync();
				return chats.Select(x => x.Chat).ToList();

			}
		}
	}
}

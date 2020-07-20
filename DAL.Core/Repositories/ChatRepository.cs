using DAL.Core.Entities;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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

		public async Task<ChatEntity> GetChat(int id, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.ChatEntities
					.Include(x => x.ChatMessageEntities)
					.ThenInclude(x => x.ChatImageContentEntities)
					.SingleOrDefaultAsync(x => x.ChatId == id, cancellationToken);
			}
		}

		public async Task<bool> CheckChatMessageExistence(Guid chatMessageUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.ChatMessageEntities.AnyAsync(x => x.ChatMessageUid == chatMessageUid, cancellationToken);
			}
		}
	}
}

using DAL.Core.Entities;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Core.Repositories
{
	public class CoreRepository : ICoreRepository
	{
		private readonly CoreContextFactory _dbContextFactory;
		public CoreRepository(CoreContextFactory dbContextFactory)
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

		public async Task<EventEntity> GetEvent(int id, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.EventEntities
					.Include(x => x.EventStatus)
					.Include(x => x.EventType)
					.Include(x => x.EventImageContent)
					.Include(x => x.Administrator)
					.Include(x => x.Participants)
						.ThenInclude(x => x.Person)
					.SingleOrDefaultAsync(x => x.EventId == id, cancellationToken);
			}
		}

		public async Task<PersonEntity> GetPerson(int id, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonEntities
					.Include(x => x.PersonImageContentEntity)
					.Include(x => x.FriendList)
						.ThenInclude(x => x.Friend)
					.Include(x => x.ChatList)
					.ThenInclude(x => x.Chat)
					.SingleOrDefaultAsync(x => x.PersonId == id, cancellationToken);
			}
		}
	}
}

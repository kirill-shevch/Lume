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
			using (var context = _dbContextFactory.CreateDbContext(new string[] { }))
			{
				return await context.ChatEntities
					.Include(x => x.ChatMessageEntities)
					.ThenInclude(x => x.ChatImageContentEntities)
					.SingleOrDefaultAsync(x => x.ChatId == id, cancellationToken).ConfigureAwait(false);
			}
		}

		public async Task<PersonEntity> GetPerson(int id, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext(new string[] { }))
			{
				return await context.PersonEntities
					.Include(x => x.personImageContentEntity)
					.Include(x => x.FriendList)
						.ThenInclude(x => x.Friend)
					.Include(x => x.ChatList)
					.ThenInclude(x => x.Chat)
					.SingleOrDefaultAsync(x => x.PersonId == id, cancellationToken).ConfigureAwait(false);
			}
		}
	}
}

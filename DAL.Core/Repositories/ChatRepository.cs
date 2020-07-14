using DAL.Core.Entities;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Core.Repositories
{
	public class ChatRepository : IChatRepository
	{
		private readonly IConfiguration _configuration;
		private readonly CoreContextFactory _dbContextFactory;
		public ChatRepository(IConfiguration configuration, CoreContextFactory dbContextFactory)
		{
			_configuration = configuration;
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
	}
}

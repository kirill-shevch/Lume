using DAL.Authorization.Entities;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Authorization
{
	public class AuthorizationRepository : IAuthorizationRepository
	{
		private readonly IConfiguration _configuration;
		private readonly AuthorizationContextFactory _dbContextFactory;
		public AuthorizationRepository(IConfiguration configuration, AuthorizationContextFactory dbContextFactory)
		{
			_configuration = configuration;
			_dbContextFactory = dbContextFactory;
		}

		public async Task AddUser(UserAuthEntity userAuthEntity, CancellationToken cancellationToken)
		{
			using (var context = _dbContextFactory.CreateDbContext(new string[] { }))
			{
				await context.AddAsync(userAuthEntity, cancellationToken).ConfigureAwait(false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}
	}
}
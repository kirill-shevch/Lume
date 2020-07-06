using Constants;
using DAL.Authorization.Entities;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Authorization
{
	public class AuthorizationRepository : IAuthorizationRepository
	{
		private readonly IConfiguration _configuration;
		public AuthorizationRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task AddUser(UserAuthEntity userAuthEntity, CancellationToken cancellationToken)
		{
			using (var context = new AuthorizationDbContext(_configuration[ConfigurationKeys.ConnectionString]))
			{
				await context.AddAsync(userAuthEntity, cancellationToken).ConfigureAwait(false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}
	}
}
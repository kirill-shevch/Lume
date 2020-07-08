using DAL.Authorization.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
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

		public async Task<UserAuthEntity> GetUser(string phoneNumber, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext(new string[] { }))
			{
				return await context.UserAuthEntities.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken).ConfigureAwait(false);
			}
		}

		public async Task<UserAuthEntity> GetUser(Guid userUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext(new string[] { }))
			{
				return await context.UserAuthEntities.SingleOrDefaultAsync(x => x.UserUid == userUid, cancellationToken).ConfigureAwait(false);
			}
		}

		public async Task UpdateUser(UserAuthEntity userAuthEntity, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext(new string[] { }))
			{
				context.Update(userAuthEntity);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}
	}
}
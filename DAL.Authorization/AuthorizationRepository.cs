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

		public async Task AddPerson(PersonAuthEntity personAuthEntity, CancellationToken cancellationToken)
		{
			using (var context = _dbContextFactory.CreateDbContext(new string[] { }))
			{
				await context.AddAsync(personAuthEntity, cancellationToken).ConfigureAwait(false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		public async Task<PersonAuthEntity> GetPerson(string phoneNumber, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext(new string[] { }))
			{
				return await context.PersonAuthEntities.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken).ConfigureAwait(false);
			}
		}

		public async Task<PersonAuthEntity> GetPerson(Guid personUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext(new string[] { }))
			{
				return await context.PersonAuthEntities.SingleOrDefaultAsync(x => x.PersonUid == personUid, cancellationToken).ConfigureAwait(false);
			}
		}

		public async Task UpdatePerson(PersonAuthEntity personAuthEntity, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext(new string[] { }))
			{
				context.Update(personAuthEntity);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}
	}
}
using DAL.Authorization.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
			using (var context = _dbContextFactory.CreateDbContext())
			{
				await context.AddAsync(personAuthEntity, cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task<PersonAuthEntity> GetPerson(string phoneNumber, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonAuthEntities.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken);
			}
		}

		public async Task<PersonAuthEntity> GetPerson(Guid personUid, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonAuthEntities.SingleOrDefaultAsync(x => x.PersonUid == personUid, cancellationToken);
			}
		}

		public async Task<List<Guid>> GetPersonListByContacts(List<string> phoneNumbers)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await context.PersonAuthEntities.Where(x => phoneNumbers.Contains(x.PhoneNumber) && !x.IsBlocked).Select(x => x.PersonUid).ToListAsync();
			}
		}

		public async Task UpdatePerson(PersonAuthEntity personAuthEntity, CancellationToken cancellationToken = default)
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				context.Update(personAuthEntity);
				await context.SaveChangesAsync(cancellationToken);
			}
		}
	}
}
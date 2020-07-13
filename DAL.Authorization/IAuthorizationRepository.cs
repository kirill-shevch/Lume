using DAL.Authorization.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Authorization
{
	public interface IAuthorizationRepository
	{
		Task AddPerson(PersonAuthEntity personAuthEntity, CancellationToken cancellationToken = default);
		Task UpdatePerson(PersonAuthEntity personAuthEntity, CancellationToken cancellationToken = default);
		Task<PersonAuthEntity> GetPerson(string phoneNumber, CancellationToken cancellationToken = default);
		Task<PersonAuthEntity> GetPerson(Guid personUid, CancellationToken cancellationToken = default);
	}
}
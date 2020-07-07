using DAL.Authorization.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Authorization
{
	public interface IAuthorizationRepository
	{
		Task AddUser(UserAuthEntity userAuthEntity, CancellationToken cancellationToken = default);

		Task UpdateUser(UserAuthEntity userAuthEntity, CancellationToken cancellationToken = default);

		Task<UserAuthEntity> GetUser(string phoneNumber, CancellationToken cancellationToken = default);
	}
}
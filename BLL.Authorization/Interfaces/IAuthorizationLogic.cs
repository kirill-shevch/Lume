using BLL.Authorization.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Authorization.Interfaces
{
	public interface IAuthorizationLogic
	{
		Task<Guid> AddUser(string code, string phoneNumber, CancellationToken cancellationToken = default);
		Task UpdateUser(string phoneNumber, string accessToken, string refreshToken, CancellationToken cancellationToken = default);
		Task<AuthorizationUserModel> GetUser(string phoneNumber, CancellationToken cancellationToken = default);
		Task SendCodeToPhone(string code, string phoneNumber, CancellationToken cancellationToken = default);
		(string AccessToken, string RefreshToken) GetTokens();
	}
}
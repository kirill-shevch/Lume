using BLL.Authorization.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Authorization.Interfaces
{
	public interface IAuthorizationLogic
	{
		Task<Guid> AddPerson(string code, string phoneNumber, CancellationToken cancellationToken = default);
		Task UpdatePerson(string phoneNumber, string accessToken, string refreshToken, CancellationToken cancellationToken = default);
		Task UpdatePerson(Guid personUid, string accessToken, string refreshToken, CancellationToken cancellationToken = default);
		Task UpdatePerson(string phoneNumber, string code, CancellationToken cancellationToken = default);
		Task<AuthorizationPersonModel> GetPerson(string phoneNumber, CancellationToken cancellationToken = default);
		Task<List<Guid>> GetPersonListByContacts(List<string> phoneNumbers);
		Task<AuthorizationPersonModel> GetPerson(Guid personUid, CancellationToken cancellationToken = default);
		Task<bool> CheckAccessKey(Guid personUid, string accessKey, CancellationToken cancellationToken = default);
		Task SendCodeToPhone(string code, string phoneNumber, CancellationToken cancellationToken = default);
		(string AccessToken, string RefreshToken) GetTokens();
		Task SendPushNotification(string code, string token);
		Task<bool> CheckThatPersonIsBlocked(Guid personUid);
	}
}
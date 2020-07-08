using BLL.Authorization.Interfaces;
using BLL.Authorization.Models;
using DAL.Authorization;
using DAL.Authorization.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Authorization
{
	public class AuthorizationLogic : IAuthorizationLogic
	{
		private readonly IAuthorizationRepository _authorizationRepository;
		public AuthorizationLogic(IAuthorizationRepository authorizationRepository)
		{
			_authorizationRepository = authorizationRepository;
		}

		public async Task<Guid> AddUser(string code, string phoneNumber, CancellationToken cancellationToken = default)
		{
			var uid = Guid.NewGuid();
			await _authorizationRepository.AddUser(new UserAuthEntity
			{
				UserUid = uid,
				TemporaryCode = code,
				PhoneNumber = phoneNumber
			}).ConfigureAwait(false);
			return uid;
		}

		public async Task<AuthorizationUserModel> GetUser(string phoneNumber, CancellationToken cancellationToken = default)
		{
			var dalUser = await _authorizationRepository.GetUser(phoneNumber, cancellationToken).ConfigureAwait(false);
			if (dalUser == null)
			{
				return null;
			}
			return new AuthorizationUserModel
			{
				PhoneNumber = dalUser.PhoneNumber,
				UserUid = dalUser.UserUid,
				Code = dalUser.TemporaryCode
			};
		}

		public async Task UpdateUser(string phoneNumber, string accessToken, string refreshToken, CancellationToken cancellationToken = default)
		{
			var dalUser = await _authorizationRepository.GetUser(phoneNumber, cancellationToken).ConfigureAwait(false);
			dalUser.AccessKey = accessToken;
			dalUser.RefreshKey = refreshToken;
			dalUser.ExpirationTime = DateTime.UtcNow.AddDays(7);
			dalUser.TemporaryCode = string.Empty;
			await _authorizationRepository.UpdateUser(dalUser, cancellationToken).ConfigureAwait(false);
		}

		public async Task UpdateUser(string phoneNumber, string code, CancellationToken cancellationToken = default)
		{
			var dalUser = await _authorizationRepository.GetUser(phoneNumber, cancellationToken).ConfigureAwait(false);
			dalUser.TemporaryCode = code;
			await _authorizationRepository.UpdateUser(dalUser, cancellationToken).ConfigureAwait(false);
		}

		public (string AccessToken, string RefreshToken) GetTokens()
		{
			return (RandomString(30), RandomString(30));
		}

		public async Task SendCodeToPhone(string code, string phoneNumber, CancellationToken cancellationToken = default)
		{
			//TODO call special service for sms sending
			//throw new NotImplementedException();
		}

		private string RandomString(int length)
		{
			var random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
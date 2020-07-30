using BLL.Authorization.Interfaces;
using BLL.Authorization.Models;
using DAL.Authorization;
using DAL.Authorization.Entities;
using System;
using System.Collections.Generic;
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

		public async Task<Guid> AddPerson(string code, string phoneNumber, CancellationToken cancellationToken = default)
		{
			var uid = Guid.NewGuid();
			await _authorizationRepository.AddPerson(new PersonAuthEntity
			{
				PersonUid = uid,
				TemporaryCode = code,
				PhoneNumber = phoneNumber
			});
			return uid;
		}

		public async Task<AuthorizationPersonModel> GetPerson(string phoneNumber, CancellationToken cancellationToken = default)
		{
			var dalPerson = await _authorizationRepository.GetPerson(phoneNumber, cancellationToken);

			return ConvertPersonModel(dalPerson);
		}

		public async Task<AuthorizationPersonModel> GetPerson(Guid personUid, CancellationToken cancellationToken = default)
		{
			var dalPerson = await _authorizationRepository.GetPerson(personUid, cancellationToken);

			return ConvertPersonModel(dalPerson);
		}

		public async Task UpdatePerson(string phoneNumber, string accessToken, string refreshToken, CancellationToken cancellationToken = default)
		{
			var dalPerson = await _authorizationRepository.GetPerson(phoneNumber, cancellationToken);
			await UpdateEntity(accessToken, refreshToken, dalPerson, cancellationToken);
		}

		public async Task UpdatePerson(Guid personUid, string accessToken, string refreshToken, CancellationToken cancellationToken = default)
		{
			var dalPerson = await _authorizationRepository.GetPerson(personUid, cancellationToken);
			await UpdateEntity(accessToken, refreshToken, dalPerson, cancellationToken);
		}

		public async Task UpdatePerson(string phoneNumber, string code, CancellationToken cancellationToken = default)
		{
			var dalPerson = await _authorizationRepository.GetPerson(phoneNumber, cancellationToken);
			dalPerson.TemporaryCode = code;
			await _authorizationRepository.UpdatePerson(dalPerson, cancellationToken);
		}

		public async Task<bool> CheckAccessKey(Guid personUid, string accessKey, CancellationToken cancellationToken = default)
		{
			var dalPerson = await _authorizationRepository.GetPerson(personUid, cancellationToken);
			if (dalPerson == null)
			{
				return false;
			}
			return dalPerson.AccessKey == accessKey && dalPerson.ExpirationTime > DateTime.UtcNow;
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

		private AuthorizationPersonModel ConvertPersonModel(PersonAuthEntity dalPerson)
		{
			if (dalPerson == null)
			{
				return null;
			}
			return new AuthorizationPersonModel
			{
				PhoneNumber = dalPerson.PhoneNumber,
				PersonUid = dalPerson.PersonUid,
				Code = dalPerson.TemporaryCode,
				RefreshToken = dalPerson.RefreshKey
			};
		}

		private async Task UpdateEntity(string accessToken, string refreshToken, PersonAuthEntity dalPerson, CancellationToken cancellationToken)
		{
			dalPerson.AccessKey = accessToken;
			dalPerson.RefreshKey = refreshToken;
			dalPerson.ExpirationTime = DateTime.UtcNow.AddDays(7);
			dalPerson.TemporaryCode = string.Empty;
			await _authorizationRepository.UpdatePerson(dalPerson, cancellationToken);
		}

		public async Task<List<Guid>> GetPersonListByContacts(List<string> phoneNumbers)
		{
			return await _authorizationRepository.GetPersonListByContacts(phoneNumbers);
		}
	}
}
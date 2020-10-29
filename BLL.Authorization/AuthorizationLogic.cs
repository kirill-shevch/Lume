using BLL.Authorization.Interfaces;
using BLL.Authorization.Models;
using BLL.Notification.Interfaces;
using Constants;
using DAL.Authorization;
using DAL.Authorization.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Authorization
{
	public class AuthorizationLogic : IAuthorizationLogic
	{
		private const string SendingRequest = @"{ ""apiKey"": ""{0}"", ""sms"": [ { ""channel"": ""char"", ""phone"": ""{1}"", ""text"": ""Lume code {2}"", ""sender"": ""VIRTA"" } ] }";

		private readonly IConfiguration _configuration;
		private readonly IAuthorizationRepository _authorizationRepository;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IPushNotificationService _pushNotificationService;
		public AuthorizationLogic(IAuthorizationRepository authorizationRepository, 
			IConfiguration configuration,
			IHttpContextAccessor contextAccessor,
			IPushNotificationService pushNotificationService)
		{
			_authorizationRepository = authorizationRepository;
			_configuration = configuration;
			_contextAccessor = contextAccessor;
			_pushNotificationService = pushNotificationService;
		}

		public async Task<Guid> AddPerson(string code, string phoneNumber, CancellationToken cancellationToken = default)
		{
			var uid = Guid.NewGuid();
			await _authorizationRepository.AddPerson(new PersonAuthEntity
			{
				PersonUid = uid,
				TemporaryCode = code,
				TemporaryCodeTime = DateTime.UtcNow,
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
			dalPerson.TemporaryCodeTime = DateTime.UtcNow;
			await _authorizationRepository.UpdatePerson(dalPerson, cancellationToken);
		}

		public async Task<bool> CheckThatPersonIsBlocked(Guid personUid)
		{
			var personEntity = await _authorizationRepository.GetPerson(personUid);
			return personEntity == null ? false : personEntity.IsBlocked;
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
			using (var httpClient = new HttpClient())
			{
				var apikey = _configuration.GetValue<string>(ConfigurationKeys.SmsSendingApiKey);
				var url = _configuration.GetValue<string>(ConfigurationKeys.SmsServiceUrl);
				var sms = new Dictionary<string, string>
				{
					{ "channel", "char" },
					{ "phone", phoneNumber },
					{ "text", string.Format("Lume code {0}", code) },
					{ "sender", "VIRTA" }
				};
				var body = new Dictionary<object, object>
				{
					{ "apiKey", apikey },
					{ "sms", new List<object> { sms } }
				};
				var content = JsonConvert.SerializeObject(body);
				var buffer = Encoding.UTF8.GetBytes(content);
				var byteContent = new ByteArrayContent(buffer);
				byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				await httpClient.PostAsync(url, byteContent);
			}
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

		public async Task SendPushNotification(string code, string token)
		{
			await _pushNotificationService.SendPushNotification(token, MessageTitles.PushNotificationMessage, null, "Lume", code);
		}
	}
}
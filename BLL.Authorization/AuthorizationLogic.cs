using BLL.Authorization.Interfaces;
using DAL.Authorization;
using DAL.Authorization.Entities;
using System;
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

		public async Task<Guid> AddUser(string code, string phoneNumber)
		{
			var uid = Guid.NewGuid();
			await _authorizationRepository.AddUser(new UserAuthEntity
			{
				UserUid = uid,
				TemporaryCode = code
			}).ConfigureAwait(false);
			return uid;
		}

		public async Task SendCodeToPhone(string code, string phoneNumber)
		{
			//throw new NotImplementedException();
		}
	}
}
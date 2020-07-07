using System;
using System.Threading.Tasks;

namespace BLL.Authorization.Interfaces
{
	public interface IAuthorizationLogic
	{
		Task<Guid> AddUser(string code, string phoneNumber);

		Task SendCodeToPhone(string code, string phoneNumber);
	}
}
using System;
using System.Threading.Tasks;

namespace BLL.Authorization.Interfaces
{
	public interface IAuthorizationLogic
	{
		Task<Guid> AddUser(string code);

		Task SendCodeToPhone(string code);
	}
}
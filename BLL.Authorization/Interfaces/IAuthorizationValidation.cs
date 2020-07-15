using BLL.Authorization.Models;

namespace BLL.Authorization.Interfaces
{
	public interface IAuthorizationValidation
	{
		(bool ValidationResult, string ValidationMessage) ValidateGetCode(string phoneNumber);
		(bool ValidationResult, string ValidationMessage) ValidateSetCode(AuthorizationPersonModel person, string code);
		(bool ValidationResult, string ValidationMessage) ValidateGetAccessToken(AuthorizationPersonModel person, string refreshToken);
	}
}

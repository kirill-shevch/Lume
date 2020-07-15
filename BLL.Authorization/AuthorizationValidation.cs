using BLL.Authorization.Interfaces;
using BLL.Authorization.Models;
using Constants;
using System.Text.RegularExpressions;

namespace BLL.Authorization
{
	public class AuthorizationValidation : IAuthorizationValidation
	{
		public (bool ValidationResult, string ValidationMessage) ValidateGetCode(string phoneNumber)
		{
			if (string.IsNullOrWhiteSpace(phoneNumber))
			{
				return (false, ErrorDictionary.GetErrorMessage(6));
			}
			else if (!Regex.Match(phoneNumber, RegexTemplates.PhoneTemplate).Success)
			{
				return (false, ErrorDictionary.GetErrorMessage(6));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateSetCode(AuthorizationPersonModel person, string code)
		{
			if (string.IsNullOrWhiteSpace(code))
			{
				return (false, ErrorDictionary.GetErrorMessage(7));
			}
			else if (!Regex.Match(code, RegexTemplates.AuthorizationCodeTemplate).Success)
			{
				return (false, ErrorDictionary.GetErrorMessage(7));
			}

			if (person == null)
			{
				return (false, ErrorDictionary.GetErrorMessage(2));
			}
			else if (person.Code != code)
			{
				return (false, ErrorDictionary.GetErrorMessage(3));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetAccessToken(AuthorizationPersonModel person, string refreshToken)
		{
			if (string.IsNullOrWhiteSpace(refreshToken))
			{
				return (false, ErrorDictionary.GetErrorMessage(8));
			}
			if (person == null)
			{
				return (false, ErrorDictionary.GetErrorMessage(2));
			}
			if (person.RefreshToken != refreshToken)
			{
				return (false, ErrorDictionary.GetErrorMessage(4));
			}

			return (true, string.Empty);
		}
	}
}

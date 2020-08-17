using BLL.Authorization.Interfaces;
using BLL.Authorization.Models;
using BLL.Core;
using Constants;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace BLL.Authorization
{
	public class AuthorizationValidation : BaseValidator, IAuthorizationValidation
	{
		public AuthorizationValidation(IHttpContextAccessor contextAccessor) : base(contextAccessor)
		{
		}
		public (bool ValidationResult, string ValidationMessage) ValidateGetCode(string phoneNumber)
		{
			if (string.IsNullOrWhiteSpace(phoneNumber))
			{
				return (false, ErrorDictionary.GetErrorMessage(6, _culture));
			}
			else if (!Regex.Match(phoneNumber, RegexTemplates.PhoneTemplate).Success)
			{
				return (false, ErrorDictionary.GetErrorMessage(6, _culture));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateSetCode(AuthorizationPersonModel person, string code)
		{
			if (string.IsNullOrWhiteSpace(code))
			{
				return (false, ErrorDictionary.GetErrorMessage(7, _culture));
			}
			else if (!Regex.Match(code, RegexTemplates.AuthorizationCodeTemplate).Success)
			{
				return (false, ErrorDictionary.GetErrorMessage(7, _culture));
			}

			if (person == null)
			{
				return (false, ErrorDictionary.GetErrorMessage(2, _culture));
			}
			else if (person.Code != code)
			{
				return (false, ErrorDictionary.GetErrorMessage(3, _culture));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetAccessToken(AuthorizationPersonModel person, string refreshToken)
		{
			if (string.IsNullOrWhiteSpace(refreshToken))
			{
				return (false, ErrorDictionary.GetErrorMessage(8, _culture));
			}
			if (person == null)
			{
				return (false, ErrorDictionary.GetErrorMessage(2, _culture));
			}
			if (person.RefreshToken != refreshToken)
			{
				return (false, ErrorDictionary.GetErrorMessage(4, _culture));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetPushCode(string phoneNumber, string token)
		{
			if (string.IsNullOrWhiteSpace(phoneNumber))
			{
				return (false, ErrorDictionary.GetErrorMessage(6, _culture));
			}
			else if (!Regex.Match(phoneNumber, RegexTemplates.PhoneTemplate).Success)
			{
				return (false, ErrorDictionary.GetErrorMessage(6, _culture));
			}
			if (string.IsNullOrWhiteSpace(token))
			{
				return (false, ErrorDictionary.GetErrorMessage(38, _culture));
			}
			return (true, string.Empty);
		}
	}
}

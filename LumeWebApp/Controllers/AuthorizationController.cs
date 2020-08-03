using BLL.Authorization.Interfaces;
using BLL.Core.Interfaces;
using LumeWebApp.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LumeWebApp.Controllers
{
	[Route("api/authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationLogic _authorizationLogic;
        private readonly IAuthorizationValidation _authorizationValidation;
        private readonly IPersonLogic _personLogic;
        public AuthorizationController(IAuthorizationLogic authorizationLogic,
            IAuthorizationValidation authorizationValidation,
            IPersonLogic personLogic)
        {
            _authorizationLogic = authorizationLogic;
            _authorizationValidation = authorizationValidation;
            _personLogic = personLogic;
        }

        [HttpPost]
        [Route("get-code")]
        public async Task<ActionResult<SignInResponse>> GetAuthorizationCode(string phoneNumber)
        {
            var validationResult = _authorizationValidation.ValidateGetCode(phoneNumber);
			if (!validationResult.ValidationResult)
			{
                return BadRequest(validationResult.ValidationMessage);
			}
            Guid personUid;
            var person = await _authorizationLogic.GetPerson(phoneNumber);
            var random = new Random();
            var code = random.Next(0, 999999).ToString("d6");
            await _authorizationLogic.SendCodeToPhone(code, phoneNumber);
            if (person == null)
            {
                personUid = await _authorizationLogic.AddPerson(code, phoneNumber);
            }
            else
            {
                personUid = person.PersonUid;
                await _authorizationLogic.UpdatePerson(phoneNumber, code);
            }
            return new SignInResponse { PersonUid = personUid };
        }

        [HttpPost]
        [Route("set-code")]
        public async Task<ActionResult<AuthorizationResponse>> SetAuthorizationCode(string phoneNumber, string code)
        {
            var person = await _authorizationLogic.GetPerson(phoneNumber);
            var validationResult = _authorizationValidation.ValidateSetCode(person, code);
            if (!validationResult.ValidationResult)
            {
                return BadRequest(validationResult.ValidationMessage);
            }
            await _personLogic.CreatePerson(person.PersonUid);
            var tokens = _authorizationLogic.GetTokens();
            await _authorizationLogic.UpdatePerson(phoneNumber, tokens.AccessToken, tokens.RefreshToken);
            return new AuthorizationResponse { AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken };
        }

        [HttpPost]
        [Route("get-access-token")]
        public async Task<ActionResult<RefreshResponse>> GetAccessToken(Guid personUid, string refreshToken)
        {
            var person = await _authorizationLogic.GetPerson(personUid);
            var validationResult = _authorizationValidation.ValidateGetAccessToken(person, refreshToken);
            if (!validationResult.ValidationResult)
            {
                return BadRequest(validationResult.ValidationMessage);
            }
            var tokens = _authorizationLogic.GetTokens();
            await _authorizationLogic.UpdatePerson(personUid, tokens.AccessToken, person.RefreshToken);
            return new RefreshResponse { AccessToken = tokens.AccessToken };
        }
    }
}
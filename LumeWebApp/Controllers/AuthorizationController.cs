using BLL.Authorization.Interfaces;
using Constants;
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
        public AuthorizationController(IAuthorizationLogic authorizationLogic)
        {
            _authorizationLogic = authorizationLogic;
        }

        [HttpPost]
        [Route("get-code")]
        public async Task<ActionResult<SignInResponse>> GetAuthorizationCode(string phoneNumber)
        {
            Guid personUid;
            var person = await _authorizationLogic.GetPerson(phoneNumber).ConfigureAwait(false);
            //TODO generate random code
            var code = "0000";
            await _authorizationLogic.SendCodeToPhone(code, phoneNumber).ConfigureAwait(false);
            if (person == null)
            {
                personUid = await _authorizationLogic.AddPerson(code, phoneNumber).ConfigureAwait(false);
            }
            else
            {
                personUid = person.PersonUid;
                await _authorizationLogic.UpdatePerson(phoneNumber, code).ConfigureAwait(false);
            }
            return new SignInResponse { PersonUid = personUid };
        }

        [HttpPost]
        [Route("set-code")]
        public async Task<ActionResult<AuthorizationResponse>> SetAuthorizationCode(string phoneNumber, string code)
        {
            var person = await _authorizationLogic.GetPerson(phoneNumber).ConfigureAwait(false);
            if (person == null)
            {
                return BadRequest(ErrorDictionary.GetErrorMessage(2));
            }
            else if (person.Code != code)
            {
                return BadRequest(ErrorDictionary.GetErrorMessage(3));
            }
            var tokens = _authorizationLogic.GetTokens();
            await _authorizationLogic.UpdatePerson(phoneNumber, tokens.AccessToken, tokens.RefreshToken).ConfigureAwait(false);
            return new AuthorizationResponse { AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken };
        }

        [HttpPost]
        [Route("get-access-token")]
        public async Task<ActionResult<RefreshResponse>> GetAccessToken(Guid personUid, string refreshToken)
        {
            var person = await _authorizationLogic.GetPerson(personUid).ConfigureAwait(false);
            if (person == null)
            {
                return BadRequest(ErrorDictionary.GetErrorMessage(2));
            }
            if (person.RefreshToken != refreshToken)
            {
                return BadRequest(ErrorDictionary.GetErrorMessage(4));
            }
            var tokens = _authorizationLogic.GetTokens();
            await _authorizationLogic.UpdatePerson(personUid, tokens.AccessToken, person.RefreshToken).ConfigureAwait(false);
            return new RefreshResponse { AccessToken = tokens.AccessToken };
        }
    }
}
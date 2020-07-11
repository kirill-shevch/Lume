using BLL.Authorization.Interfaces;
using Constants;
using LumeWebApp.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LumeWebApp.Controllers
{
	[Route("api/[controller]")]
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
            Guid userUid;
            var user = await _authorizationLogic.GetUser(phoneNumber).ConfigureAwait(false);
            //TODO generate random code
            var code = "0000";
            await _authorizationLogic.SendCodeToPhone(code, phoneNumber).ConfigureAwait(false);
            if (user == null)
            {
                userUid = await _authorizationLogic.AddUser(code, phoneNumber).ConfigureAwait(false);
            }
            else
            {
                userUid = user.UserUid;
                await _authorizationLogic.UpdateUser(phoneNumber, code).ConfigureAwait(false);
            }
            return new SignInResponse { UserUid = userUid };
        }

        [HttpPost]
        [Route("set-code")]
        public async Task<ActionResult<AuthorizationResponse>> SetAuthorizationCode(string phoneNumber, string code)
        {
            var user = await _authorizationLogic.GetUser(phoneNumber).ConfigureAwait(false);
            if (user == null)
            {
                return BadRequest(ErrorDictionary.GetErrorMessage(2));
            }
            else if (user.Code != code)
            {
                return BadRequest(ErrorDictionary.GetErrorMessage(3));
            }
            var tokens = _authorizationLogic.GetTokens();
            await _authorizationLogic.UpdateUser(phoneNumber, tokens.AccessToken, tokens.RefreshToken).ConfigureAwait(false);
            return new AuthorizationResponse { AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken };
        }

        [HttpPost]
        [Route("get-access-token")]
        public async Task<ActionResult<RefreshResponse>> GetAccessToken(Guid userUid, string refreshToken)
        {
            var user = await _authorizationLogic.GetUser(userUid).ConfigureAwait(false);
            if (user == null)
            {
                return BadRequest(ErrorDictionary.GetErrorMessage(2));
            }
            if (user.RefreshToken != refreshToken)
            {
                return BadRequest(ErrorDictionary.GetErrorMessage(4));
            }
            var tokens = _authorizationLogic.GetTokens();
            await _authorizationLogic.UpdateUser(userUid, tokens.AccessToken, user.RefreshToken).ConfigureAwait(false);
            return new RefreshResponse { AccessToken = tokens.AccessToken };
        }
    }
}
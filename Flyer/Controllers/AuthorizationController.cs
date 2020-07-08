using BLL.Authorization.Interfaces;
using Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Threading.Tasks;
using WebApi.Responses;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("authorization")]
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationLogic _authorizationLogic;
        public AuthorizationController(IAuthorizationLogic authorizationLogic)
        {
            _authorizationLogic = authorizationLogic;
        }

        [HttpPost]
        [Route("get-code")]
        [SwaggerOperation("get-code")]
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
        [SwaggerOperation("set-code")]
        public async Task<ActionResult<AuthorizationResponse>> SetAuthorizationCode(string phoneNumber, string code)
        {
            var user = await _authorizationLogic.GetUser(phoneNumber).ConfigureAwait(false);
            if (user == null)
            {
                return BadRequest(ErrorDictionaty.GetErrorMessage(2));
            }
            else if (user.Code != code)
            {
                return BadRequest(ErrorDictionaty.GetErrorMessage(3));
            }
            var tokens = _authorizationLogic.GetTokens();
            await _authorizationLogic.UpdateUser(phoneNumber, tokens.AccessToken, tokens.RefreshToken).ConfigureAwait(false);
            return new AuthorizationResponse { AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken };
        }

        [HttpPost]
        [Route("get-access-token")]
        [SwaggerOperation("get-access-token")]
        public async Task<ActionResult<RefreshResponse>> GetAccessToken(string phoneNumber, string refreshToken)
        {
            var user = await _authorizationLogic.GetUser(phoneNumber).ConfigureAwait(false);
            if (user == null)
            {
                return BadRequest(ErrorDictionaty.GetErrorMessage(2));
            }
            if (user.RefreshToken != refreshToken)
            {
                return BadRequest(ErrorDictionaty.GetErrorMessage(4));
            }
            var tokens = _authorizationLogic.GetTokens();
            await _authorizationLogic.UpdateUser(phoneNumber, tokens.AccessToken, user.RefreshToken).ConfigureAwait(false);
            return new RefreshResponse { AccessToken = tokens.AccessToken };
        }
    }
}
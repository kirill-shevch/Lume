using BLL.Authorization.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
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
        [Route("signIn")]
        [SwaggerOperation("signIn")]
        public async Task<ActionResult<SignInResponse>> SignIn(string phoneNumber)
        {
            var code = "0000";
            await _authorizationLogic.SendCodeToPhone(code, phoneNumber).ConfigureAwait(false);
            var userUid = await _authorizationLogic.AddUser(code, phoneNumber).ConfigureAwait(false);
            return new SignInResponse { UserUid = userUid };
        }
    }
}
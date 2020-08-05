using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Core.Interfaces;
using BLL.Core.Models.Person;
using Constants;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace LumeWebApp.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IPersonLogic _personLogic;
        private readonly IFriendValidation _friendValidation;

        public FriendsController(IPersonLogic personLogic,
            IFriendValidation friendValidation)
        {
            _personLogic = personLogic;
            _friendValidation = friendValidation;
        }
        
        [HttpPost]
        [Route("add-friend")]
        public async Task<ActionResult> AddFriend(Guid friendGuid)
        {
            var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());

            var validationResult = _friendValidation.ValidateAddFriend(uid, friendGuid);

            if (!validationResult.ValidationResult)
            {
                return BadRequest(validationResult.ValidationMessage);
            }

            await _personLogic.AddFriendToPerson(uid, friendGuid);

            return Ok(Messages.GetMessage(MessageTitles.FriendAdded, CultureParser.GetCultureFromHttpContext(HttpContext)));
        }

        [HttpDelete]
        [Route("remove-friend")]
        public async Task<ActionResult> RemoveFriend(Guid friendGuid)
        {
            var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());

            var validationResult = _friendValidation.ValidateRemoveFriend(uid, friendGuid);

            if (!validationResult.ValidationResult)
            {
                return BadRequest(validationResult.ValidationMessage);
            }

            await _personLogic.RemoveFriendFromPerson(uid, friendGuid);

            return Ok(Messages.GetMessage(MessageTitles.FriendRemoved, CultureParser.GetCultureFromHttpContext(HttpContext)));
        }

        [HttpGet]
        [Route("get-friends")]
        public async Task<ActionResult<List<PersonModel>>> GetFriends(Guid personUid)
        {
            return await _personLogic.GetAllPersonFriends(personUid);
        }
    }
}

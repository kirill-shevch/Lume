using BLL.Core.Interfaces;
using Constants;
using DAL.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace BLL.Core
{
    public class FriendValidation : BaseValidator, IFriendValidation
    {
        private readonly IPersonRepository _personRepository;

        public FriendValidation(IPersonRepository personRepository,
            IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _personRepository = personRepository;
        } 

        public (bool ValidationResult, string ValidationMessage) ValidateAddFriend(Guid personUid, Guid friendUid)
        {
            var isFriendExists = _personRepository.GetPerson(friendUid).Result != null;

            if (_personRepository.CheckPersonFriendExistence(personUid, friendUid).Result)
            {
                return (false, ErrorDictionary.GetErrorMessage(18, _culture));
            }
            if (!isFriendExists)
            {
                return (false, ErrorDictionary.GetErrorMessage(2, _culture));
            }

            return (true, string.Empty);
        }

        public (bool ValidationResult, string ValidationMessage) ValidateRemoveFriend(Guid personUid, Guid friendUid)
        {
            var isFriendExists = _personRepository.GetPerson(friendUid).Result != null;

            if (!_personRepository.CheckPersonFriendExistence(personUid, friendUid).Result)
            {
                return (false, ErrorDictionary.GetErrorMessage(17, _culture));
            }
            if (!isFriendExists)
            {
                return (false, ErrorDictionary.GetErrorMessage(2, _culture));
            }

            return (true, string.Empty);
        }
    }
}

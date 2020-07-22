using BLL.Core.Interfaces;
using Constants;
using DAL.Core.Interfaces;
using System;

namespace BLL.Core
{
    public class FriendValidation : IFriendValidation
    {
        private readonly IPersonRepository _personRepository;

        public FriendValidation(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        } 

        public (bool ValidationResult, string ValidationMessage) ValidateAddFriend(Guid personUid, Guid friendUid)
        {
            var isFriendExists = _personRepository.GetPerson(friendUid).Result != null;

            if (_personRepository.CheckPersonFriendExistence(personUid, friendUid).Result)
            {
                return (false, ErrorDictionary.GetErrorMessage(18));
            }
            if (!isFriendExists)
            {
                return (false, ErrorDictionary.GetErrorMessage(2));
            }

            return (true, string.Empty);
        }

        public (bool ValidationResult, string ValidationMessage) ValidateRemoveFriend(Guid personUid, Guid friendUid)
        {
            var isFriendExists = _personRepository.GetPerson(friendUid).Result != null;

            if (!_personRepository.CheckPersonFriendExistence(personUid, friendUid).Result)
            {
                return (false, ErrorDictionary.GetErrorMessage(17));
            }
            if (!isFriendExists)
            {
                return (false, ErrorDictionary.GetErrorMessage(2));
            }

            return (true, string.Empty);
        }
    }
}

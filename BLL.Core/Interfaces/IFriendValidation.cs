using System;

namespace BLL.Core.Interfaces
{
    public interface IFriendValidation
    {
        public (bool ValidationResult, string ValidationMessage) ValidateAddFriend(Guid personUid, Guid friendUid);

        public (bool ValidationResult, string ValidationMessage) ValidateRemoveFriend(Guid personUid, Guid friendUid);
    }
}

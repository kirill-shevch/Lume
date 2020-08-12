using System;

namespace BLL.Core.Interfaces
{
    public interface IFriendValidation
    {
        public (bool ValidationResult, string ValidationMessage) ValidateLackOfFriendship(Guid personUid, Guid friendUid);

        public (bool ValidationResult, string ValidationMessage) ValidateFriendship(Guid personUid, Guid friendUid);
    }
}

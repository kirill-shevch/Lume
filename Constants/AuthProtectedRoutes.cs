using System.Collections.Generic;

namespace Constants
{
    public static class AuthProtectedRoutes
    {
        public static readonly List<string> ProtectedRoutes = new List<string>
        {
             "/home/secure",

             "/api/core/get-person",
             "/api/core/update-person",
             "/api/core/is-person-filled-up",
             "/api/core/add-event",

             "/api/image/add-person-image",
             "/api/image/add-event-image"
        };
    }
}
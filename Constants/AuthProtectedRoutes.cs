using System.Collections.Generic;

namespace Constants
{
    public static class AuthProtectedRoutes
    {
        public static readonly List<string> ProtectedRoutes = new List<string>
        {
             "/home/secure",

             "/api/person/get-person",
             "/api/person/update-person",
             "/api/person/is-person-filled-up",

             "/api/event/add-event",
             "/api/event/get-event",
             "/api/event/get-event-list",
             "/api/event/update-event",

             "/api/chat/get-chat",

             "/api/image/add-person-image",
             "/api/image/add-event-image"
        };
    }
}
using System.Collections.Generic;

namespace Constants
{
    public static class AuthProtectedRoutes
    {
        public static readonly List<string> ProtectedRoutes = new List<string>
        {
             "/home/secure",

             "/api/person/get-profile",
             "/api/person/get-person",
             "/api/person/update-person",
             "/api/person/is-person-filled-up",
             "/api/person/get-person-list",

             "/api/event/add-event",
             "/api/event/get-event",
             "/api/event/get-event-list",
             "/api/event/update-event",
             "/api/event/add-event-participant",
             "/api/event/update-event-participant",
             "/api/event/remove-event-participant",
             "/api/event/get-random-event",

             "/api/friends/add-friend",
             "/api/friends/remove-friend",
             "​/api​/friends​/get-friends",

             "/api/chat/get-chat",
             "/api/chat/get-person-chat",
             "/api/chat/get-person-chat-list",
             "/api/chat/add-chat-message",
             "/api/chat/get-new-chat-messages",

             "/api/image/add-person-image",
             "/api/image/add-event-image"
        };
    }
}
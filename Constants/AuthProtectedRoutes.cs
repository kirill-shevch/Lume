using System.Collections.Generic;

namespace Constants
{
    public static class AuthProtectedRoutes
    {
        public static readonly List<string> ProtectedRoutes = new List<string>
        {
             "/secure",

             "/api/person/get-profile",
             "/api/person/get-person",
             "/api/person/update-person",
             "/api/person/is-person-filled-up",
             "/api/person/get-person-list",
             "/api/person/get-random-person",
             "/api/person/accept-random-person",
             "/api/person/reject-random-person",
             "/api/person/get-person-notifications",
             "/api/person/remove-person-token",
             "/api/person/add-feedback",
             "/api/person/get-badges",
             "/api/person/add-report",

             "/api/event/add-event",
             "/api/event/get-event",
             "/api/event/get-event-list",
             "/api/event/update-event",
             "/api/event/add-event-participant",
             "/api/event/update-event-participant",
             "/api/event/remove-event-participant",
             "/api/event/get-random-event",
             "/api/event/search-for-event",
             "/api/event/accept-random-event",
             "/api/event/reject-random-event",
             "/api/event/add-promo-reward-request",
             "/api/event/remove-event-image",
             "/api/event/add-report",

             "/api/friends/add-friend",
             "/api/friends/remove-friend",
             "​/api​/friends​/get-friends",
             "​/api​/friends​/confirm-friend",

             "/api/chat/get-chat",
             "/api/chat/get-person-chat",
             "/api/chat/get-person-chat-list",
             "/api/chat/add-chat-message",
             "/api/chat/get-new-chat-messages",
             "/api/chat/mute-chat",

             "/api/city/get-cities",
             "/api/city/check-city-for-promo-reward",

             "/api/contacts/get-person-list-by-contacts"
        };
    }
}
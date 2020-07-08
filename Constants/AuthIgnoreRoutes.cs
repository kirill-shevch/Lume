using System.Collections.Generic;

namespace Constants
{
    public static class AuthIgnoreRoutes
    {
        public static readonly List<string> IgnoredRoutes = new List<string>
        {
            "/authorization/get-code",
            "/authorization/set-code",
            "/authorization/get-access-token"
        };
    }
}

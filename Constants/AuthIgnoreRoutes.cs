using System.Collections.Generic;

namespace Constants
{
    public static class AuthIgnoreRoutes
    {
        public static readonly List<string> IgnoredRoutes = new List<string>
        {
            "/home",
            "/api/authorization/get-code",
            "/api/authorization/set-code",
            "/api/authorization/get-access-token",
            "/swagger/index.html",
            "/swagger/v2/swagger.json",
            "/swagger/favicon-32x32.png"
        };
    }
}
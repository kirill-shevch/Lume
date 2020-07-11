using System.Collections.Generic;

namespace Constants
{
    public static class AuthIgnoreRoutes
    {
        public static readonly List<string> IgnoredRoutes = new List<string>
        {
            "/home",
            "/authorization/get-code",
            "/authorization/set-code",
            "/authorization/get-access-token",
            "/swagger/index.html",
            "/swagger/v2/swagger.json",
            "/swagger/favicon-32x32.png"
        };
    }
}

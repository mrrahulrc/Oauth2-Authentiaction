using System;

namespace OauthAuthentiaction.Constants
{
    public class UnProtectedRoutes
    {
        public static readonly List<String> routes = new List<String>()
        {
            "/",
            "/home/index",
            "/auth/oauth",
            "/auth/token",
            "/error/error"
        };
    }
}

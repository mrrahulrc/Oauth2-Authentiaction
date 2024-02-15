namespace OauthAuthentiaction.Constants
{
    public class OAuthUrl
    {
        public static readonly String authCodeUrl = "https://accounts.google.com/o/oauth2/v2/auth/oauthchooseaccount";
        public static readonly String tokenUrl = "https://oauth2.googleapis.com/token";
        public static readonly String userInforUrl = "https://www.googleapis.com/oauth2/v3/userinfo?access_token={0}";
    }
}

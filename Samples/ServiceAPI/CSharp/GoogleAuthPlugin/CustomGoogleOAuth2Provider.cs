using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Text;

namespace GoogleAuthPlugin
{
    public class CustomGoogleOAuth2Provider : OAuth2Provider
    {
        public const string Name = "GoogleOAuth";

        public const string Realm = "https://accounts.google.com/o/oauth2/auth";

        public string VerifyAccessTokenUrl { get; set; } = "https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}";


        public CustomGoogleOAuth2Provider(IAppSettings appSettings)
            : base(appSettings, "https://accounts.google.com/o/oauth2/auth", "GoogleOAuth")
        {
            AuthorizeUrl = AuthorizeUrl ?? "https://accounts.google.com/o/oauth2/auth";
            AccessTokenUrl = AccessTokenUrl ?? "https://accounts.google.com/o/oauth2/token";
            UserProfileUrl = UserProfileUrl ?? "https://www.googleapis.com/oauth2/v1/userinfo";
            if (Scopes.Length == 0)
            {
                Scopes = new string[2] { "https://www.googleapis.com/auth/userinfo.profile", "https://www.googleapis.com/auth/userinfo.email" };
            }

            VerifyTokenUrl = VerifyAccessTokenUrl;
        }

        protected override Task<Dictionary<string, string>> CreateAuthInfoAsync(string accessToken, CancellationToken token = default)
        {
            JsonObject jsonObject = JsonObject.Parse(UserProfileUrl.AddQueryParam("access_token", accessToken).GetJsonFromUrl());
            return new Dictionary<string, string>
            {
                {
                    "user_id",
                    jsonObject["id"]
                },
                {
                    "username",
                    jsonObject["email"]
                },
                {
                    "email",
                    jsonObject["email"]
                },
                {
                    "name",
                    jsonObject["name"]
                },
                {
                    "first_name",
                    jsonObject["given_name"]
                },
                {
                    "last_name",
                    jsonObject["family_name"]
                },
                {
                    "gender",
                    jsonObject["gender"]
                },
                {
                    "birthday",
                    jsonObject["birthday"]
                },
                {
                    "link",
                    jsonObject["link"]
                },
                {
                    "picture",
                    jsonObject["picture"]
                },
                {
                    "locale",
                    jsonObject["locale"]
                },
                {
                    "profileUrl",
                    jsonObject["picture"]
                }
            }.AsTaskResult();
        }
    }
}
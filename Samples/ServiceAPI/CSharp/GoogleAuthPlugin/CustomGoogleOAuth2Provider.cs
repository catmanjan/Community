using System.Collections.Generic;
using ServiceStack.Configuration;
using ServiceStack.Text;

namespace ServiceStack.Authentication.OAuth2
{
    public class CustomGoogleOAuth2Provider : OAuth2Provider
    {
        public const string Name = "GoogleOAuth";

        public const string Realm = "https://accounts.google.com/o/oauth2/auth";

        public string VerifyAccessTokenUrl { get; set; } = "https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}";


        public CustomGoogleOAuth2Provider(IAppSettings appSettings)
            : base(appSettings, "https://accounts.google.com/o/oauth2/auth", "GoogleOAuth")
        {
            base.AuthorizeUrl = base.AuthorizeUrl ?? "https://accounts.google.com/o/oauth2/auth";
            base.AccessTokenUrl = base.AccessTokenUrl ?? "https://accounts.google.com/o/oauth2/token";
            base.UserProfileUrl = base.UserProfileUrl ?? "https://www.googleapis.com/oauth2/v1/userinfo";
            if (base.Scopes.Length == 0)
            {
                base.Scopes = new string[2] { "https://www.googleapis.com/auth/userinfo.profile", "https://www.googleapis.com/auth/userinfo.email" };
            }

            base.VerifyAccessToken = OnVerifyAccessToken;
        }

        public bool OnVerifyAccessToken(string accessToken)
        {
            return true;
        }

        protected override Dictionary<string, string> CreateAuthInfo(string accessToken)
        {
            JsonObject jsonObject = JsonObject.Parse(base.UserProfileUrl.AddQueryParam("access_token", accessToken).GetJsonFromUrl());
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
            };
        }
    }
}
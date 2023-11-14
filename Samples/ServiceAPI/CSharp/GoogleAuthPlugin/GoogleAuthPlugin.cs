using HP.HPTRIM.Service;
using ServiceStack;
using ServiceStack.Auth;
using System.Linq;

namespace GoogleAuthPlugin
{
    public class Reconfigure : IReconfigure
    {
        public void Configure(IAppHost appHost)
        {
            var appSettings = new ServiceStack.Configuration.AppSettings();

            if (appHost.Plugins.Exists(p => p is AuthFeature))
            {
                appHost.Plugins.Remove(appHost.Plugins.First(p => p is AuthFeature));
            }

            string oauthLogin = appSettings.GetString("oauth.login");

            if (oauthLogin == null)
            {
                oauthLogin = "~/auth/google";
            }

            /*appHost.Plugins.Add(new AuthFeature(() => new AuthUserSession(), new IAuthProvider[] {
                        new CustomGoogleOAuth2Provider(appSettings),
                    }, oauthLogin));*/

            appHost.Plugins.Add(new AuthFeature(() => new AuthUserSession(), new IAuthProvider[] {
                         new GoogleAuthProvider(appSettings)
                         {
                             /*AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth",
                             AccessTokenUrl = "https://accounts.google.com/o/oauth2/token",
                             UserProfileUrl = "https://www.googleapis.com/oauth2/v1/userinfo",
                             VerifyTokenUrl = "https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}",*/
                             VerifyAccessTokenAsync = null
                         },
                     }, oauthLogin));
        }
    }
}

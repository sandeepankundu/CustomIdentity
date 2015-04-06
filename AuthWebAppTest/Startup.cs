using AuthWebAppTest.AuthRelated;
using AuthWebAppTest.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using NHibernate;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(AuthWebAppTest.Startup))]
namespace AuthWebAppTest
{
    public class Startup
    {
        
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }



        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, AngularJSAuthentication.API.Migrations.Configuration>());
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);


            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            //Configure Google External Login
            //googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "1071357982354-flb6jmenlphtmkrqstm5qrdv37ljjlaa.apps.googleusercontent.com",
            //    ClientSecret = "6m61nIRbKVRhRK5FP3VzJoMF",
            //    Provider = new GoogleAuthProvider()
            //};
            //app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            //facebookAuthOptions = new FacebookAuthenticationOptions()
            //{
            //    AppId = "xxx",
            //    AppSecret = "xxx",
            //    Provider = new FacebookAuthProvider()
            //};
            //app.UseFacebookAuthentication(facebookAuthOptions);


        }
    }
}
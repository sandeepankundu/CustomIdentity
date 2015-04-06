using AuthWebAppTest.Services;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using NHibernate;
using NHibernate.AspNet.Identity;
using NHibernate.AspNet.Identity.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AuthWebAppTest.AuthRelated
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private const int PASSWORD_HISTORY_LIMIT = 5;

        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            PasswordValidator = new CustomPasswordValidator(10);// new MinimumLengthValidator(10);// enforcing password length restriction
            UserValidator = new CustomUserValidator<ApplicationUser>(this); 
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {

            var usrEntities = new[] {
                typeof(IdentityUser)
            };

            string cnctnString = ConfigurationManager.ConnectionStrings["AuthContext"].ConnectionString;
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(cnctnString)).CurrentSessionContext("web")
                //.Mappings(m => m.FluentMappings.AddFromAssemblyOf<SqlCommandFactory>())
                .ExposeConfiguration(cfg =>
                {
                    cfg.AddDeserializedMapping(MappingHelper.GetIdentityMappings(usrEntities), null);
                })
                .BuildSessionFactory();
            ISession session = sessionFactory.OpenSession();



            //var appDbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(session));

            // Configure validation logic for usernames
            appUserManager.UserValidator = new UserValidator<ApplicationUser>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            //appUserManager.EmailService = new AspNetIdentity.WebApi.Services.EmailService();
            appUserManager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }

            return appUserManager;
        }

        public override async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            if (await IsPreviousPassword(userId, newPassword))
            {
                return await Task.FromResult(IdentityResult.Failed("Cannot reuse old password"));
            }
            var result = await base.ChangePasswordAsync(userId, currentPassword, newPassword);
            if (result.Succeeded)
            {
                var store = Store as ApplicationUserStore;
                await store.AddToPreviousPasswordsAsync(await FindByIdAsync(userId), PasswordHasher.HashPassword(newPassword));
            }
            return result;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            if (await IsPreviousPassword(userId, newPassword))
            {
                return await Task.FromResult(IdentityResult.Failed("Cannot reuse old password"));
            }
            var result = await base.ResetPasswordAsync(userId, token, newPassword);
            if (result.Succeeded)
            {
                var store = Store as ApplicationUserStore;
                await store.AddToPreviousPasswordsAsync(await FindByIdAsync(userId), PasswordHasher.HashPassword(newPassword));
            }
            return result;
        }

        private async Task<bool> IsPreviousPassword(string userId, string newPassword)
        {
            var user = await FindByIdAsync(userId);
            if (user.PreviousUserPasswords.OrderByDescending(x => x.CreateDate).
                Select(x => x.PasswordHash).Take(PASSWORD_HISTORY_LIMIT).Where(x => PasswordHasher.VerifyHashedPassword(x, newPassword) != PasswordVerificationResult.Failed
                ).Any())
            {
                return true;
            }
            return false;
        }
    }
}
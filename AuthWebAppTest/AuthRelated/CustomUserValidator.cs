using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AuthWebAppTest.AuthRelated
{
    ///ref: http://blachniet.com/blog/email-addresses-as-user-names-in-asp-net-identity/
    ///
    /// <summary>
    /// A replacement for the <see cref="UserValidator"/> which requires that an email 
    /// address be used for the <see cref="IUser.UserName"/> field.
    /// </summary>
    /// <typeparam name="TUser">Must be a type derived from <see cref="Microsoft.AspNet.Identity.IUser"/>.</typeparam>
    /// <remarks>
    /// This validator check the <see cref="IUser.UserName"/> property against the simple email regex provided at
    /// http://www.regular-expressions.info/email.html. If a <see cref="UserManager"/> is provided in the constructor,
    /// it will also ensure that the email address is not already being used by another account in the manager.
    /// 
    /// To use this validator, just set <see cref="UserManager.UserValidator"/> to a new instance of this class.
    /// </remarks>
    public class CustomUserValidator<TUser> : IIdentityValidator<TUser>
        where TUser : class, Microsoft.AspNet.Identity.IUser
    {
        private static readonly Regex EmailRegex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly UserManager<TUser> _manager;

        public CustomUserValidator()
        {
        }

        public CustomUserValidator(UserManager<TUser> manager)
        {
            _manager = manager;
        }

        public async Task<IdentityResult> ValidateAsync(TUser item)
        {
            var errors = new List<string>();
            if (!EmailRegex.IsMatch(item.UserName))
                errors.Add("Enter a valid email address.");

            if (_manager != null)
            {
                var otherAccount = await _manager.FindByNameAsync(item.UserName);
                if (otherAccount != null && otherAccount.Id != item.Id)
                    errors.Add("Select a different email address. An account has already been created with this email address.");
            }

            return errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success;
        }
    }
}
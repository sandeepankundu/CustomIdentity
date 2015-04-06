using NHibernate;
using NHibernate.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AuthWebAppTest.AuthRelated
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(ISession context)
            : base(context)
        {

        }

        public override async Task CreateAsync(ApplicationUser user)
        {
            await base.CreateAsync(user);
            await AddToPreviousPasswordsAsync(user, user.PasswordHash);
        }

        public Task AddToPreviousPasswordsAsync(ApplicationUser user, string password)
        {
            user.PreviousUserPasswords.Add(new PreviousPassword() { UserId = user.Id, PasswordHash = password });
            return UpdateAsync(user);
        }
    }
}
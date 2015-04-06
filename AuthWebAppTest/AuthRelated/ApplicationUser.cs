using NHibernate.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthWebAppTest.AuthRelated
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
            : base()
        {
            PreviousUserPasswords = new List<PreviousPassword>();
        }

        public virtual IList<PreviousPassword> PreviousUserPasswords { get; set; }
    }
}
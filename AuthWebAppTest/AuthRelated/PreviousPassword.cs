using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AuthWebAppTest.AuthRelated
{
    public class PreviousPassword
    {
        public PreviousPassword()
        {
            CreateDate = DateTimeOffset.Now;
        }
        [Key, Column(Order = 0)]
        public string PasswordHash { get; set; }
        public DateTimeOffset CreateDate { get; set; }

        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
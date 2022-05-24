using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySpace.Models
{
    public partial class UserSubscriptionWrapper
    {
        public string AccountType { get; set; }
        public User User { get; set; }
    }
}
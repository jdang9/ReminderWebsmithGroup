using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageSlips.Models
{
    public class UsersViewModel
    {
        public IEnumerable<User> Users { get; set; }
    }
}

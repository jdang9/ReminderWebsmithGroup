﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageSlips.Models
{
    public class Login
    {
        public IEnumerable<User> GetUsers { get; set; }
    }
}
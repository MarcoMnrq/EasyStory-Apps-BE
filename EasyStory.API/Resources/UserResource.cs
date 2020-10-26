﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyStory.API.Resources
{
    public class UserResource
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AccountBalance { get; set; }
        public int SubscriptionPrice { get; set; }
    }
}
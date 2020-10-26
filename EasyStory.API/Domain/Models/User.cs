﻿using System;
using System.Collections.Generic;

namespace EasyStory.API.Domain.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AccountBalance { get; set; }
        public int SubscriptionPrice { get; set; }

        public IList<Post> Posts { get; set; } = new List<Post>();
        public IList<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }
}
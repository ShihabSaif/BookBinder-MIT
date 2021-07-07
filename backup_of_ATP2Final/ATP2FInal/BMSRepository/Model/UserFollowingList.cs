using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMSApi.Models
{
    public class UserFollowingList
    {
        public string user_name { get; set; }
        public int MainUserId { get; set; }
        public int GuestUserId { get; set; }
    }
}
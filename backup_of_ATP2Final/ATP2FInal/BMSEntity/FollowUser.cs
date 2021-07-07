using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSEntity
{
    public class FollowUser
    {
        public int FollowUserId { get; set; }
        public int MainUserId { get; set; }
        public int GuestUserId { get; set; }

    }
}

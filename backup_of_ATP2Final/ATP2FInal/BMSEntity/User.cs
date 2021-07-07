using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSEntity
{
    public class User
    {
        public int userId { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string image_link { get; set; }
        public int BookCount { get; set; }
        public double AverageRating { get; set; }
    }
}

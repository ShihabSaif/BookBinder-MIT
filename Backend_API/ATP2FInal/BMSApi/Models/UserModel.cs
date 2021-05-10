using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMSApi.Models
{
    public class UserModel
    {
        public int userId { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public HttpPostedFileBase image_link { get; set; }
        public string image_link_name { get; set; }
        public int BookCount { get; set; }
        public double AverageRating { get; set; }
    }
}
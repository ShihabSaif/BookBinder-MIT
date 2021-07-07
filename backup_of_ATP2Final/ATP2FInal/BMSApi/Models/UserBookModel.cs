using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMSApi.Models
{
    public class UserBookModel
    {
        public int userBookId { get; set; }
        //[Key, ForeignKey("user")]
        public int userId { get; set; }
        //[Key, ForeignKey("book")]
        public int BookId { get; set; }
        public int readStatus { get; set; }
        public int Rating { get; set; }
    }
}
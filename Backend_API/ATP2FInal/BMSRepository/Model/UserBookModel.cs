using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSRepository.Model
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

        public String bookTitle { get; set; }

        public String userName { get; set; }
    }
}

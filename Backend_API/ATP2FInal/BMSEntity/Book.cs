using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSEntity
{
    public class Book
    {
        public int BookId { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string image_link { get; set; }
        public double review { get; set; }
    }
}

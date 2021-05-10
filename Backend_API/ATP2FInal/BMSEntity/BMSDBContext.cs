using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSEntity
{
    public class BMSDBContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBook> userBooks { get; set; }
        public DbSet<FollowUser> followUsers { get; set; }
    }
}

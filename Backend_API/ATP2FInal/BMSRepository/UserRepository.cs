using BMSEntity;
using BMSRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSRepository
{
    public class UserRepository
    {
        private BMSDBContext context = new BMSDBContext();

        public List<User> GetAll()
        {
            return context.Users.ToList();
        }

        public List<FollowUser> GetFollowUser()
        {
            return context.followUsers.ToList();
        }

        public List<UserBookModel> GetAllFollowUser(int id)
        {
            List<UserBook> ls = new List<UserBook>();
            //ls = select BookId,readStatus,Rating FROM UserBooks INNER JOIN FollowUsers on UserBooks.userId = FollowUsers.GuestUserId WHERE FollowUsers.MainUserId = 3;

            //ls = (((List<UserBook>)(from userbook in context.userBooks
            //                        join followuser in context.followUsers on userbook.userBookId equals followuser.GuestUserId
            //                        where followuser.MainUserId == id
            //                        select new { Post = userbook })));

            var innerJoin = context.userBooks.Join(// outer sequence 
                      context.followUsers,  // inner sequence 
                      userbook => userbook.userId,    // outerKeySelector
                      followuser => followuser.GuestUserId,  // innerKeySelector

                      (userbook, followuser) => new
                      {
                          userbook.BookId,
                          userbook.readStatus,
                          userbook.Rating,
                          followuser.MainUserId,
                          followuser.GuestUserId
                      }).Where(x => x.MainUserId == id).
                      Join(context.Books, uf => uf.BookId, book => book.BookId, (uf, book) => new

                      {
                          uf.GuestUserId,
                          uf.BookId,
                          uf.readStatus,
                          uf.Rating,
                          uf.MainUserId,
                          book.title

                      }).Join(context.Users, userFollow => userFollow.GuestUserId, user => user.userId, (userFollow, user) => new UserBookModel
                      {
                          BookId = userFollow.BookId,
                          readStatus = userFollow.readStatus,
                          Rating = userFollow.Rating,
                          bookTitle = userFollow.title,
                          userName = user.user_name
                      }).ToList();
                     
                      
                      //new UserBookModel  // result selector
                      //{
                      //    BookId = userbook.BookId,
                      //    readStatus = userbook.readStatus,
                      //    Rating = userbook.Rating,
                      //    userId = followuser.MainUserId
                      //}).Where(x => x.userId == id).ToList();

        //    var UserInRole = db.UserProfiles.
        //Join(db.UsersInRoles, u => u.UserId, uir => uir.UserId,
        //(u, uir) => new { u, uir }).
        //Join(db.Roles, r => r.uir.RoleId, ro => ro.RoleId, (r, ro) => new { r, ro })
        //.Where(m => m.r.u.UserId == 1)
        //.Select(m => new AddUserToRole
        //{
        //    UserName = m.r.u.UserName,
        //    RoleName = m.ro.RoleName
        //});

            //List<UserBook> ls = query.ToList<UserBook>();
            return innerJoin;
        }

        public User Get(int id)
        {
            return context.Users.SingleOrDefault(b => b.userId == id);
        }

        public User GetName(string name)
        {
            return context.Users.SingleOrDefault(b => b.email == name);
        }

        public int Insert(User bk)
        {
            context.Users.Add(bk);
            return context.SaveChanges();
        }

        public String InsertFollowingUser(int MainUserId, int GuestUserId)
        {
            FollowUser bk = context.followUsers.Where(u => u.MainUserId == MainUserId && u.GuestUserId == GuestUserId).FirstOrDefault();
            if (bk != null)
            {
                return "false";
            }
            else
            {
                FollowUser follow = new FollowUser();

                follow.MainUserId = MainUserId;
                follow.GuestUserId = GuestUserId;

                context.followUsers.Add(follow);

                context.SaveChanges();

                return "true";
            }
            

            //context.followUsers.Add(fu);
            //return context.SaveChanges();
        }

        public int deleteFollowingUsers(int MainUserId, int GuestUserId)
        {

            var remove = (from aremove in context.followUsers
                          where aremove.MainUserId == MainUserId
                             && aremove.GuestUserId == GuestUserId
                          select aremove).FirstOrDefault();

            context.followUsers.Remove(remove);
            return context.SaveChanges();

            //context.followUsers.Add(fu);
            //return context.SaveChanges();
        }

        public int followUserInsert(FollowUser fu)
        {
            context.followUsers.Add(fu);
            return context.SaveChanges();
        }

        public List<User> getSearchedUser(String user)
        {
            
            var result = from s in context.Users
                         where s.user_name == user
                         select s;

            List<User> searchedUser = result.ToList();
            //User usr = context.Users.SingleOrDefault(b => b.user_name == user);
            return searchedUser;
        }


        public User Update(User user, int id)
        {
            User usr = context.Users.SingleOrDefault(b => b.userId == id);

            if (usr != null)
            {
                if (user.user_name != null)
                    usr.user_name = user.user_name;
                if (user.password != null)
                    usr.password = user.password;
                if (user.image_link != null)
                    usr.image_link = user.image_link;
                if (user.BookCount != 0)
                    usr.BookCount = user.BookCount;
                if (user.AverageRating != 0)
                    usr.AverageRating = user.AverageRating;

                context.SaveChanges();

                return usr;
            }
            else
                return null;

        }

        public int Delete(int id)
        {
            User bk = context.Users.SingleOrDefault(b => b.userId == id);
            if (bk != null)
            {
                context.Users.Remove(bk);
            }
            return context.SaveChanges();
        }

        public User GetEmailNdPass(string email, string password)
        {
            User bk = context.Users.Where(u => u.email == email && u.password == password).FirstOrDefault();
            if (bk != null)
            {
                return bk;
            }
            else
                return null;
        }

        public bool DuplicateMail(string email)
        {
            //return context.Users.Any(u => u.email == us.email);
            if (context.Users.Any(u => u.email == email))
            {
                return true;
            }
            else
                return false;
        }
    }
}


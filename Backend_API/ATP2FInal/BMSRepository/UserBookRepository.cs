using BMSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSRepository
{
    public class UserBookRepository
    {
        private BMSDBContext context = new BMSDBContext();
        private UserRepository repo = new UserRepository();
        private BookRepository repoBook = new BookRepository();

        public int Delete(int id)
        {
            UserBook bk = context.userBooks.SingleOrDefault(b => b.userBookId == id);
            if (bk != null)
            {
                context.userBooks.Remove(bk);
            }

            GetBookCount(bk.userId);
            GetAverageRatingForUser(bk.userId);
            GetAverageRating(bk.BookId);

            return context.SaveChanges();
        }

        public double GetAverageRating(int id)
        {
            //userBook bk = context.userBooks.First(b => b.BookId == id);

            if (context.userBooks.Any(b => b.BookId == id && b.Rating != 0))
            {
                double rating = context.userBooks.Where(c => c.BookId == id && c.Rating != 0)
                                    .Average(c => c.Rating);

                Book bk = new Book
                {
                    review = rating
                };
                repoBook.Update(bk, id);

                return rating;
            }
            else
            {
                return 0.000;
            }
        }

        public double GetAverageRatingForUser(int id)
        {
            //userBook bk = context.userBooks.First(b => b.BookId == id);

            if (context.userBooks.Any(b => b.userId == id && b.Rating != 0))
            {
                double rating = context.userBooks.Where(c => c.userId == id && c.Rating != 0)
                                    .Average(c => c.Rating);

                User user = new User
                {
                    AverageRating = rating
                };
                repo.Update(user, id);

                return rating;
            }
            else
            {
                return 0.000;
            }
        }

        public int GetBookCount(int id)
        {
            //userBook bk = context.userBooks.First(b => b.BookId == id);

            if (context.userBooks.Any(b => b.userId == id))
            {
                int count = context.userBooks.Where(c => c.userId == id)
                                    .Count();

                User user = new User
                {
                    BookCount = count
                };
                repo.Update(user, id);

                return count;
            }
            else
            {
                return 0;
            }
        }

        public UserBook Get(int id)
        {
            return context.userBooks.SingleOrDefault(b => b.userBookId == id);
        }

        public List<UserBook> GetByBookId(int id)
        {
            return context.userBooks.Where(b => b.BookId == id).ToList();
        }

        public UserBook GetUserAndBookId(int UserId, int BookId)
        {
            UserBook ub = context.userBooks.SingleOrDefault(b => b.BookId == BookId && b.userId == UserId);

            if (ub != null)
            {
                return ub;
            }
            else
            {
                return null;
            }
        }

        public List<UserBook> GetAll()
        {
            return context.userBooks.ToList();
        }

        public int Insert(UserBook bk)
        {
            context.userBooks.Add(bk);
            int value = context.SaveChanges();

            GetBookCount(bk.userId);
            GetAverageRatingForUser(bk.userId);
            GetAverageRating(bk.BookId);

            return value;
        }

        //public int UpdateRating(userBook book)
        //{
        //    userBook bk = context.userBooks.SingleOrDefault(b => b.userBookId == book.userBookId);

        //    //bk.userId = book.userId;
        //    //bk.BookId = book.BookId;
        //    //bk.readStatus = book.readStatus;
        //    bk.Rating = book.Rating;

        //    return context.SaveChanges();
        //}

        //public int UpdateStatus(userBook book)
        //{
        //    userBook bk = context.userBooks.SingleOrDefault(b => b.userBookId == book.userBookId);

        //    //bk.userId = book.userId;
        //    //bk.BookId = book.BookId;
        //    //bk.readStatus = book.readStatus;
        //    bk.readStatus = book.readStatus;

        //    return context.SaveChanges();
        //}

        public UserBook Update(UserBook book, int id)
        {
            UserBook bk = context.userBooks.SingleOrDefault(b => b.userBookId == id);

            if (bk != null)
            {
                if (book.userId != 0)
                    bk.userId = book.userId;
                if (book.BookId != 0)
                    bk.BookId = book.BookId;
                if (book.readStatus != 0)
                    bk.readStatus = book.readStatus;
                if (book.Rating != 0)
                    bk.Rating = book.Rating;

                context.SaveChanges();

                GetBookCount(bk.userId);
                GetAverageRatingForUser(bk.userId);
                GetAverageRating(bk.BookId);

                return bk;
            }
            else
                return null;

        }

        public List<Book> GetUserBookList(int userId, int readStatus)
        {
            List<Book> list;
            if (readStatus == 0)
            {
                list = (from d in context.userBooks
                        join f in context.Books
                        on d.BookId equals f.BookId
                        where d.userId == userId &&
                        (d.readStatus == 1 || d.readStatus == 2 || d.readStatus == 3 || d.readStatus == 0)
                        select f).ToList();
            }
            else
            {
                list = (from d in context.userBooks
                        join f in context.Books
                        on d.BookId equals f.BookId
                        where d.userId == userId && d.readStatus == readStatus
                        select f).ToList();
            }

            if (list != null)
            {
                return list;
            }
            else
            {
                return null;
            }
        }
    }
}


using BMSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSRepository
{
    public class BookRepository
    {
        private BMSDBContext context = new BMSDBContext();

        public List<Book> GetAll()
        {
            return context.Books.ToList();
        }

        public Book Get(int id)
        {
            return context.Books.SingleOrDefault(b => b.BookId == id);
        }

        public int Insert(Book bk)
        {
            context.Books.Add(bk);
            return context.SaveChanges();
        }

        public Book Update(Book book, int id)
        {
            Book bk = context.Books.SingleOrDefault(b => b.BookId == id);

            if (bk != null)
            {
                if (book.title != null)
                    bk.title = book.title;
                if (book.author != null)
                    bk.author = book.author;
                if (book.category != null)
                    bk.category = book.category;
                if (book.description != null)
                    bk.description = book.description;
                if (book.image_link != null)
                    bk.image_link = book.image_link;
                if (book.review != 0)
                    bk.review = book.review;

                context.SaveChanges();

                return bk;
            }
            else
                return null;

        }

        public int Delete(int id)
        {
            Book bk = context.Books.SingleOrDefault(b => b.BookId == id);
            if (bk != null)
            {
                context.Books.Remove(bk);
            }

            UserBookRepository repo = new UserBookRepository();
            List<UserBook> list = repo.GetByBookId(id);
            foreach (UserBook i in list)
            {
                repo.Delete(i.userBookId);
            }

            return context.SaveChanges();
        }

        public List<Book> BookSearch(string name)
        {
            var obj = (context.Books.Where(o => o.title.Contains(name) || o.author.Contains(name)));
            if (obj != null)
                return obj.ToList();
            else
                return null;
        }

        public List<Book> GetDescendingListByCategory(string name)
        {
            List<Book> list = context.Books.OrderByDescending(date => date.review).ToList();
            if (list != null)
                return list;
            else
                return null;
        }

        public List<Book> DistinctCategory()
        {
            List<Book> uniq = context.Books.GroupBy(x => x.category).Select(y => y.FirstOrDefault()).ToList();

            //return context.Books.ToList();

            if (uniq != null)
                return uniq;
            else
                return null;
        }

        public List<string> DistinctCategoryString()
        {
            List<Book> uniq = context.Books.GroupBy(x => x.category).Select(y => y.FirstOrDefault()).ToList();
            List<string> list = new List<string>();
            foreach (var i in uniq)
            {
                list.Add(i.category);
            }
            //return context.Books.ToList();

            if (list != null)
                return list;
            else
                return null;
        }

        public List<Book> GetCategory(string category)
        {
            List<Book> bk = context.Books.Where(b => b.category == category).ToList();

            if (bk != null)
                return bk;
            else
                return null;
        }

        public int rating(double str, int bookId)
        {
            Book bk = context.Books.SingleOrDefault(b => b.BookId == bookId);
            bk.review = str;
            return context.SaveChanges();
        }
    }
}


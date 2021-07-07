using BMSEntity;
using BMSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BMSApi.Controllers
{
        [System.Web.Http.RoutePrefix("api/Book")]
        public class BookController : ApiController
        {
            BookRepository repo = new BookRepository();
            private BMSDBContext context = new BMSDBContext();

            [System.Web.Http.HttpGet]
            public IHttpActionResult Get()
            {
                List<Book> list = repo.GetAll();

                if (list != null)
                    return Ok(list);
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }


            [System.Web.Http.Route("{id}")]
            [System.Web.Http.HttpGet]
            public IHttpActionResult Get(int id)
            {
                Book bk = repo.Get(id);

                if (bk != null)
                    return Ok(bk);
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }

            [BasicAuthentication]
            public IHttpActionResult Post(Book bk)
            {
                repo.Insert(bk);

                return Created("", bk);
            }

            [BasicAuthentication]
            [System.Web.Http.Route("{id}")]
            [System.Web.Http.HttpPut]
            public IHttpActionResult Put(Book bk, int id)
            {
                Book book = repo.Update(bk, id);

                if (book != null)
                    return Ok(book);
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }

            [BasicAuthentication]
            [System.Web.Http.Route("{id}")]
            [System.Web.Http.HttpDelete]
            public IHttpActionResult Delete(int id)
            {
                repo.Delete(id);
                return StatusCode(HttpStatusCode.NoContent);
            }


            [System.Web.Http.Route("search/{name}")]
            [System.Web.Http.HttpGet]
            public IHttpActionResult Search(string name)
            {
                List<Book> list = repo.BookSearch(name);

                if (list != null)
                    return Ok(list);
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }

            [System.Web.Http.Route("category/{name}")]
            [System.Web.Http.HttpGet]
            public IHttpActionResult ListByCategory(string name)
            {
                List<Book> list = repo.GetCategory(name);

                if (list != null)
                    return Ok(list);
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }

            [System.Web.Http.Route("uniqueCategory")]
            [System.Web.Http.HttpGet]
            public IHttpActionResult UniqueCategory()
            {
                List<string> list = repo.DistinctCategoryString();

                if (list != null)
                    return Ok(list);
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }

            //[Route("{bookId}/{str}")]
            //public IHttpActionResult GetRating(double str, int bookId)
            //{
            //    return Ok(repo.rating(str,bookId));
            //}
        }
    }

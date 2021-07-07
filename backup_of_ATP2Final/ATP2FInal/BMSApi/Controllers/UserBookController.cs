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
    
        [BasicAuthentication]
        [System.Web.Http.RoutePrefix("api/UserBook")]
        public class UserBookController : ApiController
        {
            UserBookRepository repo = new UserBookRepository();

            [System.Web.Http.HttpGet]
            public IHttpActionResult Get()
            {
                List<UserBook> list = repo.GetAll();

                if (list != null)
                    return Ok(list);
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }


            [System.Web.Http.Route("{id}")]
            [System.Web.Http.HttpGet]
            public IHttpActionResult Get(int id)
            {
                UserBook bk = repo.Get(id);

                if (bk != null)
                    return Ok(bk);
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }

            [System.Web.Http.Route("AverageRating/{id}")]
            [System.Web.Http.HttpGet]
            public IHttpActionResult GetAverageRating(int id)
            {
                double rating = repo.GetAverageRating(id);

                return Ok(rating);
            }

            [System.Web.Http.Route("AverageRatingForUser/{id}")]
            [System.Web.Http.HttpGet]
            public IHttpActionResult GetAverageRatingForUser(int id)
            {
                double rating = repo.GetAverageRatingForUser(id);

                return Ok(rating);
            }

            //for user
            [System.Web.Http.Route("BookCount/{id}")]
            [System.Web.Http.HttpGet]
            public IHttpActionResult GetBookCount(int id)
            {
                int count = repo.GetBookCount(id);

                return Ok(count);
            }

            [System.Web.Http.Route("UserAndBookId/{uid}/{bid}")]
            [System.Web.Http.HttpGet]
            public IHttpActionResult GetUserAndBookId(int uid, int bid)
            {
                UserBook us = repo.GetUserAndBookId(uid, bid);

                if (us != null)
                    return Ok(us);
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }

            [System.Web.Http.Route("UserBookList/{uid}/{status}")]
            [System.Web.Http.HttpGet]
            public IHttpActionResult GetUserBookList(int uid, int status)
            {
                List<Book> us = repo.GetUserBookList(uid, status);

                if (us != null)
                    return Ok(us);
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }

            public IHttpActionResult Post(UserBook us)
            {
                repo.Insert(us);

                return Created("", us);
            }

            [System.Web.Http.Route("{id}")]
            [System.Web.Http.HttpPut]
            public IHttpActionResult Put(UserBook us, int id)
            {
                UserBook book = repo.Update(us, id);

                if (book != null)
                    return Ok(book);
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }

            [System.Web.Http.Route("{id}")]
            [System.Web.Http.HttpDelete]
            public IHttpActionResult Delete(int id)
            {
                repo.Delete(id);
                return StatusCode(HttpStatusCode.NoContent);
            }
        }
    }

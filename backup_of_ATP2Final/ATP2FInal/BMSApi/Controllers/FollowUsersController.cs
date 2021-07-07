using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BMSApi.Models;
using BMSEntity;
using BMSRepository;
using BMSRepository.Model;

namespace BMSApi.Controllers
{
    [System.Web.Http.RoutePrefix("api/FollowUser")]
    public class FollowUsersController : ApiController
    {
        private BMSDBContext db = new BMSDBContext();
        UserRepository repo = new UserRepository();

        [System.Web.Http.Route("{id}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetUserFollowing(int id)
        {
            List<BMSRepository.Model.UserBookModel> list = repo.GetAllFollowUser(id); //ambiguous model between api.model and repository.model

            if (list != null)
                return Ok(list);
            else
                return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("{AllFollow}/{id}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetAllFollowing(int id)
        {
            List<UserFollowingList> list = repo.GetFollowingList(id);

            if (list != null)
                return Ok(list);
            else
                return StatusCode(HttpStatusCode.NoContent);
        }



        [System.Web.Http.Route("{MainUserId}/{GuestUserId}")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult postUserFollowing(int MainUserId, int GuestUserId)
        {
            String result = repo.InsertFollowingUser(MainUserId, GuestUserId);
            return Ok(result);
            
        }

        [System.Web.Http.Route("{MainUserId}/{GuestUserId}")]
        [System.Web.Http.HttpDelete]
        public IHttpActionResult postUserunFollowing(int MainUserId, int GuestUserId)
        {
            int result = repo.deleteFollowingUsers(MainUserId, GuestUserId);
            return Ok(result);

        }



        // GET: api/FollowUsers/5
        /*[ResponseType(typeof(FollowUser))]
        public IHttpActionResult GetFollowUser(int id)
        {
            FollowUser followUser = db.followUsers.Find(id);
            if (followUser == null)
            {
                return NotFound();
            }

            return Ok(followUser);
        }

        // PUT: api/FollowUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFollowUser(int id, FollowUser followUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != followUser.FollowUserId)
            {
                return BadRequest();
            }

            db.Entry(followUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FollowUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/FollowUsers
        [ResponseType(typeof(FollowUser))]
        public IHttpActionResult PostFollowUser(FollowUser followUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.followUsers.Add(followUser);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = followUser.FollowUserId }, followUser);
        }

        // DELETE: api/FollowUsers/5
        [ResponseType(typeof(FollowUser))]
        public IHttpActionResult DeleteFollowUser(int id)
        {
            FollowUser followUser = db.followUsers.Find(id);
            if (followUser == null)
            {
                return NotFound();
            }

            db.followUsers.Remove(followUser);
            db.SaveChanges();

            return Ok(followUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FollowUserExists(int id)
        {
            return db.followUsers.Count(e => e.FollowUserId == id) > 0;
        }
        */
    }
}
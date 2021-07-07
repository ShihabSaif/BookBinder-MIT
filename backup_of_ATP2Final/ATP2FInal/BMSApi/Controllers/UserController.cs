using BMSEntity;
using BMSRepository;
using BMSRepository.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BMSApi.Controllers
{
    [System.Web.Http.RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        BMSDBContext context = new BMSDBContext();
        UserRepository repo = new UserRepository();
        string useremail = Thread.CurrentPrincipal.Identity.Name;

        //[BasicAuthentication]
        [System.Web.Http.HttpGet]
        public IHttpActionResult Get()
        {
            List<User> list = repo.GetAll();

            if (list != null)
                return Ok(list);
            else
                return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("AdminFileApproval")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult AdminFileApproval()
        {
            //return "OK";
            List<Book> bookFile = repo.GetAdminFileApproval();
            return Ok(bookFile);
        }

        [System.Web.Http.Route("FileStatusApprove/{bookId}")]
        [System.Web.Http.HttpPut]
        public IHttpActionResult FileStatusApprove(int bookId)
        {
            //return "OK";
            bool bookFile = repo.GetFileStatusApprove(bookId);
            return Ok(bookFile);
        }

        [System.Web.Http.Route("download/{id}")]
        [System.Web.Http.HttpGet]
        public string Download(int id)
        {
            //return "OK";
            Book bk = context.Books.SingleOrDefault(b => b.BookId == id);
            if(bk.status==0)
            {
                return "false";
            }
            else
            {
                return bk.image_link;
            }
        }

        [System.Web.Http.Route("upload/{id}")]
        [System.Web.Http.HttpPost]
        public Task<HttpResponseMessage> Upload(int id)
        {
            List<string> savedFilePath = new List<string>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string rootPath = HttpContext.Current.Server.MapPath("~/uploadFiles");
            var provider = new MultipartFileStreamProvider(rootPath);
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsCanceled || t.IsFaulted)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }
                    foreach (MultipartFileData item in provider.FileData)
                    {
                        try
                        {
                            string name = item.Headers.ContentDisposition.FileName.Replace("\"", "");
                            string newFileName = Guid.NewGuid() + "-" + Path.GetFileName(name);
                            File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));

                            Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                            string fileRelativePath = "~/uploadFiles/" + newFileName;
                            Uri fileFullPath = new Uri(baseuri, VirtualPathUtility.ToAbsolute(fileRelativePath));
                            savedFilePath.Add(fileFullPath.ToString());

                            Book usr = context.Books.SingleOrDefault(b => b.BookId == id);
                            usr.image_link = savedFilePath[0];
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }
                    }
               
                    return Request.CreateResponse(HttpStatusCode.Created, savedFilePath);
                });

            

            return task ;
        }


        [System.Web.Http.Route("{search}/{user}")]
        public IHttpActionResult gettingsearcheduser(string user)
        {
            List<User> us = repo.getSearchedUser(user);
            return Ok(us);
        }

        //[System.Web.Http.Route("{follow}/{id}")]
        //[System.Web.Http.HttpGet]
        //public IHttpActionResult GetUserFollowing(int id)
        //{
        //    List<UserBookModel> list = repo.GetAllFollowUser(id);

        //    if (list != null)
        //        return Ok(list);
        //    else
        //        return StatusCode(HttpStatusCode.NoContent);
        //}

        //[BasicAuthentication]
        //[System.Web.Http.HttpGet]
        //[System.Web.Http.Route("{follow}/{follo}/{foll}")]
        //public IHttpActionResult GetFollowUser()
        //{
        //    List<FollowUser> FollowList = repo.GetFollowUser();

        //    if (FollowList != null)
        //        return Ok(FollowList);
        //    else
        //        return StatusCode(HttpStatusCode.NoContent);
        //}


        //[BasicAuthentication]
        [System.Web.Http.HttpPost]
        public IHttpActionResult Post(User bk)
        {
            repo.Insert(bk);
            return Created("", bk);
        }

        //[System.Web.Http.HttpPost]
        //[System.Web.Http.Route("{follow}/{mainUserId}/{FollowingUserId}")]
        //public IHttpActionResult PostFollowingUser(FollowUser bk)
        //{
        //    repo.InsertFollowingUser(bk);
        //    return Created("", bk);
        //}

        //[System.Web.Http.HttpPost]
        //public IHttpActionResult UserFollowing(FollowUser fu)
        //{
        //    repo.followUserInsert(fu);
        //    return Created("",fu);
        //}

        [BasicAuthentication]
        [System.Web.Http.Route("{id}")]
        [System.Web.Http.HttpPut]
        public IHttpActionResult Put(User bk, int id)
        {
            User user = repo.Update(bk, id);

            if (user != null)
                return Ok(user);
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

        //[BasicAuthentication]
        [System.Web.Http.Route("{id}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult Get(int id)
        {
            User usr = repo.Get(id);

            if (usr != null)
                return Ok(usr);
            else
                return StatusCode(HttpStatusCode.NoContent);
        } 

        

        [BasicAuthentication]
        [System.Web.Http.Route("email/{email}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetName(string email)
        {
            User usr = repo.GetName(email);

            if (usr != null)
                return Ok(usr);
            else
                return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("logincheck")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetEmailNdPass(string email, string password)
        {
            User usr = repo.GetEmailNdPass(email, password);

            if (usr != null)
                return Ok(usr);
            else
                return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("duplicateMail")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetDuplicateEmail(string email)
        {
            //List<User> usr = repo.DuplicateMail(us);
            bool exist = repo.DuplicateMail(email);

            if (exist)
                return Ok();
            else
                return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
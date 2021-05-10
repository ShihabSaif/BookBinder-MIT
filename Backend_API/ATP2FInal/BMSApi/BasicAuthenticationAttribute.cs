using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BMSApi
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string encodedCredential = actionContext.Request.Headers.Authorization.Parameter;
                string decodedCredential = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredential));
                string[] arr = decodedCredential.Split(':');
                string email = arr[0];
                string password = arr[1];

                if (LogIn.Login(email, password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(email), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}
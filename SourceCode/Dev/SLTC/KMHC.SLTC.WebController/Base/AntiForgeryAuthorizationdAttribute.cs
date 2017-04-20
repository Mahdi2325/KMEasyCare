using System;
using System.Web;
using System.Web.Mvc;
using KMHC.Infrastructure.Security;
using System.Security.Principal;
using System.Web.Routing;
using KM.Common;
 
namespace KMHC.SLTC.WebUI.Controllers.Base
{
    public class AntiForgeryAuthorizationdAttribute : AuthorizeAttribute
    {
        private readonly IAuthenticationService _authenticationService = IOCContainer.Instance.Resolve<IAuthenticationService>();
        protected virtual ICustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as ICustomPrincipal; }
        }

        public AntiForgeryAuthorizationdAttribute()
        {
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            //IHCustomPrincipal user = (IHCustomPrincipal)httpContext.User;
            //if (!user.Identity.IsAuthenticated)
            //{
            //    return false;
            //}

            if (!DetermineAccessAllow(httpContext))
            {
                return false;
            }

            return base.AuthorizeCore(httpContext);
        }

        /// <summary>
        /// Determine current request if can access the action.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private bool DetermineAccessAllow(HttpContextBase httpContext)
        {
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //string sLoginUrl = "/Home/login";

            //filterContext.Result = new RedirectResult(sLoginUrl);

            //base.HandleUnauthorizedRequest(filterContext);

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "action", "login" },
                    { "controller", "Home" },
                    { "isFromAuth", "true" }
                }
            );
        }


        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (!_authenticationService.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new
                     RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            //filterContext.Result = new RedirectToRouteResult(new
            //        RouteValueDictionary(new { Controller = "Home", Action = "Error", ErrorMessage = "您没有权限访问此页面！" }));
        }
    }
}

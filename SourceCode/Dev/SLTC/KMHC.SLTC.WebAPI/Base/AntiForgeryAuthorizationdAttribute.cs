using KM.Common;
using KMHC.Infrastructure.Security;
using KMHC.SLTC.Business.Entity;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.Base
{
    public class AntiForgeryAuthorizationdAttribute : AuthorizeAttribute
    {
        private readonly IAuthenticationService _authenticationService = IOCContainer.Instance.Resolve<IAuthenticationService>();

        //public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        //{
        //    if (!SecurityHelper.IsAuthenticated)
        //    {
        //        base.OnAuthorization(actionContext);
        //    }
        //}

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            return _authenticationService.IsAuthenticated;
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
            actionContext.Response.Content = new StringContent("{ \"ResultMessage\": \"没有权限访问\", \"ResultCode\": " + (int)EnumResponseStatus.Unauthorized + " }",
                Encoding.GetEncoding("UTF-8"), "application/json");
            //actionContext.Response.Content =
            //if (!actionContext.Controller.User.Identity.IsAuthenticated)
            //{
            //    if (filterContext.HttpContext.Request.IsAjaxRequest())
            //    {
            //        filterContext.Result = new JsonResult
            //        {
            //            Data = new { Message = "Your session has died a terrible and gruesome death" },
            //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //        };
            //        filterContext.HttpContext.Response.StatusCode = 401;
            //        filterContext.HttpContext.Response.StatusDescription = "Humans and robots must authenticate";
            //        filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            //    }
            //}
            //base.HandleUnauthorizedRequest(actionContext);
        }
    }
}
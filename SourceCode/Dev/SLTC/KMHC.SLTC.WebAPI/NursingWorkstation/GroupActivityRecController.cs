using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
     [RoutePrefix("api/GroupActivityRec")]
    public class GroupActivityRecController : BaseController
    {
         INursingWorkstationService service = IOCContainer.Instance.Resolve<INursingWorkstationService>();

         [Route(""), HttpGet]
         public IHttpActionResult Query(string activityName, int currentPage = 1, int pageSize = 10)
         {
             BaseRequest<GroupActivityRecFilter> request = new BaseRequest<GroupActivityRecFilter>();
             request.CurrentPage = currentPage;
             request.PageSize = pageSize;
             request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
             request.Data.ActivityName = activityName;
             var response = service.QueryGroupActivityRec(request);
             return Ok(response);
         }

         [Route("{id}")]
         public IHttpActionResult Get(int id)
         {
             var response = service.GetGroupActivityRec(id);
             return Ok(response);
         }

         [Route("")]
         public IHttpActionResult Post(GroupActivityRec baseRequest)
         {
             var response = service.SaveGroupActivityRec(baseRequest);
             return Ok(response);
         }

         [Route("{id}")]
         public IHttpActionResult Delete(int id)
         {
             var response = service.DeleteGroupActivityRec(id);
             return Ok(response);
         }
    }
}

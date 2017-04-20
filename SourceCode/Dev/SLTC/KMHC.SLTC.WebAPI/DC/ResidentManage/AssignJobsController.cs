using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC
{
   [RoutePrefix("api/AssignJobs")]
    public class AssignJobsController : BaseController
    {
       IDC_AssignJobsService service = IOCContainer.Instance.Resolve<IDC_AssignJobsService>();
       [Route(""), HttpGet]
       public IHttpActionResult Get(int CurrentPage, int PageSize, bool? RecStatus = null, bool? NewRecFlag = null, DateTime? AssignStartDate = null, DateTime? AssignEndDate = null)
       {
           BaseRequest<DC_AssignJobsFilter> request = new BaseRequest<DC_AssignJobsFilter>();
           request.CurrentPage = CurrentPage;
           request.PageSize = PageSize;
            request.Data.RecStatus = RecStatus;     
            request.Data.NewRecFlag = NewRecFlag;
            request.Data.AssignStartDate = AssignStartDate;
            request.Data.AssignEndDate = AssignEndDate;
           request.Data.Assignee = SecurityHelper.CurrentPrincipal.EmpNo;
           var response = service.QueryTaskList(request);
           return Ok(response);
       }
       [Route(""), HttpGet]
       public object QueryAssignTask(DateTime start, DateTime end)
       {

           BaseRequest<AssignTaskJobFilter> request = new BaseRequest<AssignTaskJobFilter>
           {

               Data = { start = start, end = end, Assignee = SecurityHelper.CurrentPrincipal.EmpNo }
           };
           return service.QueryAssTask(request);

       }
       [Route("")]
       public IHttpActionResult Get(int Id, bool recStatus, DateTime? finishDate=null)
       {
           var response = service.ChangeRecStatus(Id, recStatus, finishDate);
             return Ok(response);
       }

       [Route("")]
       public IHttpActionResult Get()
       {
           var response = service.GetGlobalVariable();
           return Ok(response);
       }


       [Route("")]
       public IHttpActionResult Post(DC_TaskRemind baseRequest)
       {
           var response = service.SaveTaskRemind(baseRequest);
           return Ok(response);
       }


    }
}

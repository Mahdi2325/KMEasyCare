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

namespace KMHC.SLTC.WebAPI.NursingWorkstation
{
    [RoutePrefix("api/AssignTask")]
    public class AssignTaskController : BaseController
    {
        INursingWorkstationService AssignTaskService = IOCContainer.Instance.Resolve<INursingWorkstationService>();

        // GET api/Floor
        [Route(""), HttpGet]
        public IHttpActionResult get(int CurrentPage, int PageSize, long feeNo)
        {
            BaseRequest<AssignTaskFilter> request = new BaseRequest<AssignTaskFilter>
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = { FeeNo = feeNo, OrgId = SecurityHelper.CurrentPrincipal.OrgId }
            };
            var response = AssignTaskService.QueryAssignTask(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(BaseRequest<AssignTaskFilter> request)
        {
            var response = AssignTaskService.QueryAssignTask(request);
            return Ok(response.Data);
        }


        // GET api/syteminfo/5
        [Route("{Id}")]
        public IHttpActionResult Get(int Id)
        {
            var response = AssignTaskService.GetAssignTask(Id);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(AssignTask baseRequest)
        {
            if (string.IsNullOrEmpty(baseRequest.Assignee))
            {
                baseRequest.Assignee = SecurityHelper.CurrentPrincipal.EmpNo;
            }
            if (string.IsNullOrEmpty(baseRequest.AssignName))
            {
                baseRequest.AssignName = SecurityHelper.CurrentPrincipal.EmpName;
            }
            var response = AssignTaskService.SaveAssignTask(baseRequest);
            return Ok(response);
        }
        [Route("assSave")]
        public IHttpActionResult Post(AssignTask2 baseRequest)
        {
            var response = AssignTaskService.SaveAssignTask2(baseRequest);
            return Ok(response);
        }

        // POST api/syteminfo
        [Route("multisave")]
        public IHttpActionResult Post(List<AssignTask> baseRequest)
        {
            var response = AssignTaskService.SaveAssignTask(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{Id}")]
        public IHttpActionResult Delete(int Id)
        {
            var response = AssignTaskService.DeleteAssignTask(Id);
            return Ok(response);
        }
    }

}

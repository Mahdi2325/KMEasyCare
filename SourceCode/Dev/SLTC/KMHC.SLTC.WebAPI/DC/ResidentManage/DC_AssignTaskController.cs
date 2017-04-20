using KM.Common;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.ResidentManage
{
    [RoutePrefix("api/DC_AssignTask")]
    public class DC_AssignTaskController : BaseController
    {
        IDC_AssignJobsService service = IOCContainer.Instance.Resolve<IDC_AssignJobsService>();
        [Route("")]
        public IHttpActionResult Get()
        {
            var response = service.QueryEmpList();
            return Ok(response);
        }


        [Route("")]
        public IHttpActionResult Post(DC_ReAllocateTaskModel list)
        {
            var response = service.SaveAllocateTask(list.ID, list.empList);
            return Ok(response);
        }
    }
}

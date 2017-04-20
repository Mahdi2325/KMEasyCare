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
    [RoutePrefix("api/dc_AssignWordNote")]
    public class DC_AssignWorkNoteController : BaseController
    {
        IDC_AssignJobsService service = IOCContainer.Instance.Resolve<IDC_AssignJobsService>();
        [Route("")]
        public IHttpActionResult Post(DC_ReAllocateTaskModel list)
        {
            var response = service.SaveAssignWorkNote(list);
            return Ok(response);
        }
    }
}

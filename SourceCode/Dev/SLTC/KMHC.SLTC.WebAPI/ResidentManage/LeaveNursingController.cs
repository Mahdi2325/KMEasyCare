using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.ResidentManage
{
    [RoutePrefix("api/LeaveNursing")]
    public class LeaveNursingController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get([FromUri]long feeNo)
        {
            var response = service.GetLeaveNursing(feeNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(Resident resident)
        {
            //resident.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveLeaveNursing(resident);
            return Ok(response);
        }
    }
}

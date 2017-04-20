using KM.Common;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.NurseCare
{
    [RoutePrefix("api/DC_REGCPL2")]
    public class DC_NursingPlanActionController : BaseController
    {
        IDC_ResidentManageService service = IOCContainer.Instance.Resolve<IDC_ResidentManageService>();
        [Route("")]
        public IHttpActionResult post(List<DC_RegCpl> requestList)
        {
            var response = service.saveRegCplEval(requestList);
            return Ok(response);
        }
    }
}

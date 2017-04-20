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
       [RoutePrefix("api/DC_ASSESSVALUE")]
    public class DC_AssessValueController : BaseController
    {
        IDC_ResidentManageService service = IOCContainer.Instance.Resolve<IDC_ResidentManageService>();
                 
        [Route(""), HttpGet]
        public IHttpActionResult get(long seqNo)
        {
            var response = service.QueryAssessValue(seqNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult post(ASSESSVALUE request)
        {
            var response = service.saveAssessValue(request);
            return Ok(response);
        }
    }
}

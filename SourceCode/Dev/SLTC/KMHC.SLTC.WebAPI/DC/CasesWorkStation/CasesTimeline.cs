using KM.Common;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.CasesWorkStation
{
    [RoutePrefix("api/casesTimeline")]
    public class CasesTimeline : BaseController
    {
        IDC_TransdisciplinaryPlan service = IOCContainer.Instance.Resolve<IDC_TransdisciplinaryPlan>();

        [Route("")]
        public IHttpActionResult Get(long feeNo, string name, string startDate, string endDate, int tag)
        {
            var response = service.QueryCasesTimeline(feeNo, startDate, endDate, tag);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Get(long feeNo)
        {
           // var response = service.QueryCasesTimeline(feeNo, startDate, endDate, tag);
            return Ok();
        }

    }
}

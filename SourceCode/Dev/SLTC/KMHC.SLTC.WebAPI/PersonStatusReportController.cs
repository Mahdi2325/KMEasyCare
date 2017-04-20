using KMHC.SLTC.Business.Entity.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using KM.Common;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/PersonStatusReport")]
    public class PersonStatusReportController : BaseController
    {
        readonly IPersonStatusReportService service = IOCContainer.Instance.Resolve<IPersonStatusReportService>();

        [Route("")]
        public IHttpActionResult Post(PersonStatusFilter baseRequest)
        {
            var response = service.QueryPersonStatusInfo(baseRequest);
            return Ok(response);
        }
    }
}

using KM.Common;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{

    [RoutePrefix("api/NurseDailyReportPipe")]
    public class NurseDailyReportPipeController : BaseController
    {

        readonly INursingRecord _INursingRecord = IOCContainer.Instance.Resolve<INursingRecord>();
        /// <summary>
        ///加载管路
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get(int feeno)
        {
            var response = _INursingRecord.GetNurseDailyReportpipe(feeno);

            return Ok(response);
        }
    }
}

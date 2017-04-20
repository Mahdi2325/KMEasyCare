using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NursingEvaluation
{
     [RoutePrefix("api/nurDemandHis")]
    public class NurDemandHisController : BaseController
    {
        INursingManageService nursingSvc = IOCContainer.Instance.Resolve<INursingManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query([FromUri]NursingFilter request)
        {
            try
            {
                var response = nursingSvc.QueryCareDemandHis(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("{id:long}")]
        public IHttpActionResult Get([FromUri]long id)
        {
            var response = nursingSvc.QueryLatestCareDemand(id);
            return Ok(response);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            try
            {
                var response = nursingSvc.DeleteNurDemandEval(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

    }
}

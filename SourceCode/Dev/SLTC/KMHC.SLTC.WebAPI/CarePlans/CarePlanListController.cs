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

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/carePlanList")] 
    public class CarePlanListController : BaseController
    {
        ICarePlansManageService carePlansSvc = IOCContainer.Instance.Resolve<ICarePlansManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query([FromUri]CarePlansFilter request)
        {
            try
            {
                var response = carePlansSvc.QueryCarePlanList(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("")]
        public IHttpActionResult Get([FromUri]long seqNo)
        {
            try
            {
                var response = carePlansSvc.GetCarePlan(seqNo);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }


        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = carePlansSvc.DeleteActivity(id);
            return Ok(response);
        }
    }
}

using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
     [RoutePrefix("api/carePlanActivityRes")] 
    public class CarePlanActivityController : BaseController
    {
        ICarePlansManageService carePlansSvc = IOCContainer.Instance.Resolve<ICarePlansManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query([FromUri]long seqNo)
        {
            try
            {
                var response = carePlansSvc.GetCareActivityList(seqNo);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("")]
        public IHttpActionResult Get([FromUri]int cp_no)
        {
            try
            {
                var response = carePlansSvc.GetPlanActivityLP(cp_no);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }
        [Route("GetById"), HttpGet]
        public IHttpActionResult GetById([FromUri]int id)
        {
            try
            {
                var response = carePlansSvc.GetActivity(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("")]
        public IHttpActionResult Post(LTC_NSCPLActivity baseRequest)
        {
            try
            {
                var response = carePlansSvc.SaveActivity(baseRequest);
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

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
     [RoutePrefix("api/carePlanGoalRes")] 
    public class CarePlanGoalController : BaseController
    {
        ICarePlansManageService carePlansSvc = IOCContainer.Instance.Resolve<ICarePlansManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query([FromUri]long seqNo)
        {
            try
            {
                var response = carePlansSvc.GetCareGoalList(seqNo);
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
                var response = carePlansSvc.GetPlanGoalsLP(cp_no);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }


  

        [Route("")]
        public IHttpActionResult Post(NSCPLGOAL baseRequest)
        {
            try
            {
                var response = carePlansSvc.SaveGoal(baseRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        //[Route("")]
        //public IHttpActionResult Post(IList<LTC_NSCPLGOAL> request)
        //{
        //    try
        //    {
        //        // var response = carePlansSvc.SaveGoal(baseRequest);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //        LogHelper.WriteError(ex.ToString());
        //        return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
        //    }
        //}

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = carePlansSvc.DeleteGoal(id);
            return Ok(response);
        }
    }
}

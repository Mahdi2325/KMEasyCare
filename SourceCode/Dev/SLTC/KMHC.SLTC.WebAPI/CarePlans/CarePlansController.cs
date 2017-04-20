using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
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
    [RoutePrefix("api/carePlanRes")] 
    public class CarePlansController : BaseController
    {
        ICarePlansManageService carePlansSvc = IOCContainer.Instance.Resolve<ICarePlansManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query([FromUri]string allCategory)
        {
            try
            {
                BaseResponse<string> response = new BaseResponse<string>();
                var OrgId = SecurityHelper.CurrentPrincipal.EmpGroup;
                response.Data = OrgId;
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

      
        [Route("")]
        public IHttpActionResult Get([FromUri]string category)
        {
            try
            {
                var response = carePlansSvc.GetLevelPRCategory(category);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get([FromUri]string category, [FromUri]string levelPR)
        {
            try
            {
                var response = carePlansSvc.GetDiaPR(category, levelPR);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }
          [Route("")]
        public IHttpActionResult Get([FromUri]int cp_no, bool isReason)
        {
            try
            {
                object response;
                if(isReason)
                {
                    response = carePlansSvc.GetCP_Reason(cp_no);
                }
                else
                {
                    response = carePlansSvc.GetCP_Data(cp_no);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("")]
        public IHttpActionResult Get([FromUri]int cp_no_d)
        {
            try
            {
                var response = carePlansSvc.GetCP_Data(cp_no_d);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        //// POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(NSCPL baseRequest)
        {
            try
            {
                var response = carePlansSvc.SaveNSCPL(baseRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        ////// DELETE api/syteminfo/5
        //[Route("{ID}")]
        //public IHttpActionResult Delete(string ID)
        //{
        //    try
        //    {
        //        var response = injectionSvc.DeleteInjection(ID);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(ex.ToString());
        //        return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
        //    }
        //}
 
    }
}

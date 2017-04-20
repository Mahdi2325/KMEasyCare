using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
     [RoutePrefix("api/nursing")]
    public class NursingController : BaseController
    {
         INursingManageService nursingSvc = IOCContainer.Instance.Resolve<INursingManageService>();
      
         
        [Route(""), HttpGet]
         public IHttpActionResult Query([FromUri]NursingFilter request)
        {
            try
            {
                var response = nursingSvc.QueryEvaluationList(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("")]
        public IHttpActionResult Get(string Code)
        {
            try
            {
                BaseResponse<QUESTION> response = nursingSvc.GetQuetionByCode(Code);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }


        [Route("")]
        public IHttpActionResult Get([FromUri]int qId, [FromUri]long? regNo, [FromUri]long? recordId)
        {
            try
            {
                BaseResponse<QUESTION> response = nursingSvc.GetQuetion(qId, regNo, recordId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("")]
        public IHttpActionResult Get([FromUri]long recordId)
        {
            try
            {
                BaseResponse<REGQUESTION> response = nursingSvc.GetREGQuetion(recordId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

    



        [Route("")]
        public IHttpActionResult Post(REGQUESTION baseRequest)
        {
            try
            {
                var response = nursingSvc.SaveQuetion(baseRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }


        // GET api/syteminfo/5
        [Route("{id:long}")]
        public object Get(int id)
        {
            BaseResponse<object> response = new BaseResponse<object>();
           // response.Data = getList()[id - 1];
            return Ok();
        }


    }
}

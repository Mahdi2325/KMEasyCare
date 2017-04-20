using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NursingEvaluation
{
     [RoutePrefix("api/nurDemandEval")]
    public class NurDemandEvalController : BaseController
    {
         INursingManageService nursingSvc = IOCContainer.Instance.Resolve<INursingManageService>();
         [Route(""), HttpGet]
         public IHttpActionResult Query([FromUri]NursingFilter request)
         {
             try
             {
                 var response = nursingSvc.QueryNurDemandEvalList(request);
                 return Ok(response);
             }
             catch (Exception ex)
             {
                 LogHelper.WriteError(ex.ToString());
                 return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
             }
         }

         [Route("")]
         public IHttpActionResult Get(long feeNo)
         {
             try
             {
                 var response = nursingSvc.QueryEvaluationResult(feeNo);
                 return Ok(response);
             }
             catch (Exception ex)
             {
                 LogHelper.WriteError(ex.ToString());
                 return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
             }
         }

         //带出相关信息


         [Route("")]
         public IHttpActionResult Get(int regon)
         {
             try
             {
                 var response = nursingSvc.QueryPerson(regon);
                 return Ok(response);
             }
             catch (Exception ex)
             {
                 LogHelper.WriteError(ex.ToString());
                 return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
             }
         }


         [Route("")]
         public IHttpActionResult Post(CareDemandEval baseRequest)
         {
             try
             {
                 var response = nursingSvc.SaveCareDemand(baseRequest);
                 return Ok(response);
             }
             catch (Exception ex)
             {
                 throw new Exception(ex.ToString());
                 LogHelper.WriteError(ex.ToString());
                 return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
             }
         }

         [Route("")]
         public IHttpActionResult Get(long demandId,string getDemand="")
         {
             try
             {
                 var response = nursingSvc.GetCareDemand(demandId);
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


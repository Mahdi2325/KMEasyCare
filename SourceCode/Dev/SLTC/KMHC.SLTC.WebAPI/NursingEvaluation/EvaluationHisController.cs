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
     [RoutePrefix("api/evaluationHis")]
    public class EvaluationHisController : BaseController
    {
        INursingManageService nursingSvc = IOCContainer.Instance.Resolve<INursingManageService>();
         [Route(""), HttpGet]
        public IHttpActionResult get(int QuestionId, long FeeNo,int CurrentPage,int PageSize)
        {
            try
            {
                BaseRequest<NursingFilter> request = new BaseRequest<NursingFilter>();
                request.CurrentPage = CurrentPage;
                request.PageSize = PageSize;
                request.Data.FeeNo = FeeNo;
                request.Data.QuestionId = QuestionId;

                var response = nursingSvc.GetEvaluationHisOver(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

         [Route("")]
         public IHttpActionResult Delete(long recId)
         {
             try
             {
                 var response = nursingSvc.DeleteEvaluation(recId);
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

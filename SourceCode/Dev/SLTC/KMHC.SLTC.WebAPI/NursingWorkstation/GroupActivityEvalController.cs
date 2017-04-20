using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NursingWorkstation
{
     [RoutePrefix("api/GroupActivityEval")]
    public class GroupActivityEvalController : BaseController
    {
         INursingManageService nursingSvc = IOCContainer.Instance.Resolve<INursingManageService>();

         [Route("")]
         public IHttpActionResult Post(RegQuestionList questionList)
         {
             try
             {
                 var response = nursingSvc.SaveBatchQuetion(questionList.Data);
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

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

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/evaluation")]
    public class EvaluationController : BaseController
    {
        INursingManageService nursingSvc = IOCContainer.Instance.Resolve<INursingManageService>();
        [Route("")]
        public IHttpActionResult Post(QUESTION question)
        {
            try
            {
                Calculation response = nursingSvc.CalcEvaluation(question);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("")]
        public IHttpActionResult Get(long feeNo, int quetionId)
        {
            try
            {
                var response = nursingSvc.GetLatestRegQuetion(feeNo, quetionId);
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

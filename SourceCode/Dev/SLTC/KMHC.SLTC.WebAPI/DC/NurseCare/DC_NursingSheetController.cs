using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.NurseCare
{
    [RoutePrefix("api/nursingSheet")]
    public class DC_NursingSheetController : BaseController
    {
        IDC_ResidentManageService service = IOCContainer.Instance.Resolve<IDC_ResidentManageService>();

        [Route("")]
        public IHttpActionResult Get([FromUri]string questionCode, [FromUri]long? regNo, [FromUri]long? recordId)
        {
            try
            {
                //BaseResponse<QUESTION> response = service.GetQuetion(qId,  recordId);
                BaseResponse<QUESTION> response = service.GetQuetionByCode(questionCode, recordId);
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
                BaseResponse<EVALQUESTION> response = service.GetREGQuetion(recordId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("")]
        public IHttpActionResult Get([FromUri]long Id, string mark)
        {
            try
            {
                BaseResponse<IList<EVALQUESTION>> response = service.GetREGQuetionList(Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }
        [Route("")]
        public IHttpActionResult Get([FromUri]List<int> l)
        {
            try
            {

                BaseResponse<string> response = new BaseResponse<string>
                {
                    Data = service.GetREGQuetionScore(l)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("")]
        public IHttpActionResult Post(EVALQUESTION baseRequest)
        {
            try
            {
                var response = service.SaveQuetion(baseRequest);
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

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

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/nursingCareEval")]
    public class NursingCareEvalController : BaseController
    {
        INursingCareEvalService service = IOCContainer.Instance.Resolve<INursingCareEvalService>();

        [Route("")]
        public IHttpActionResult Get(string Code,int feeNo, int evaluateid)
        {
            try
            {
                var response = service.GetNursingCareEvalQues(Code, feeNo, evaluateid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("")]
        public IHttpActionResult Post(ResBasicInfo baseRequest)
        {
            try
            {
                var response = service.SaveResBasicInfo(baseRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route(""), HttpGet]
        public IHttpActionResult get(long feeNo, int CurrentPage, int PageSize)
        {
            try
            {
                BaseRequest<NursingFilter> request = new BaseRequest<NursingFilter>();
                request.CurrentPage = CurrentPage;
                request.PageSize = PageSize;
                request.Data.FeeNo = feeNo;

                var response = service.QueryCareEvalInfo(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        [Route("")]
        public IHttpActionResult Delete(long evalId)
        {
            try
            {
                var response = service.DeleteEvalCare(evalId);
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

using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.NurseCare
{
    [RoutePrefix("api/DCAdjuvantTherapy")]
    public class DC_RegActivityRequestEvalController : BaseController
    {

        IDC_ResidentManageService service = IOCContainer.Instance.Resolve<IDC_ResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]int CurrentPage, int PageSize, long feeNo)
        {
            DC_RegActivityRequestEvalFilter filter = new DC_RegActivityRequestEvalFilter
            {
                FeeNo = feeNo
            };
            BaseRequest<DC_RegActivityRequestEvalFilter> request = new BaseRequest<DC_RegActivityRequestEvalFilter>
            {
                Data = filter
            };
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            var response = service.QueryRegActivityRequestEval(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult get(long feeNo, string regNo)
        {
            DC_RegActivityRequestEvalFilter filter = new DC_RegActivityRequestEvalFilter
            {
                FeeNo = feeNo,
                RegNo = regNo,
            };
            BaseRequest<DC_RegActivityRequestEvalFilter> request = new BaseRequest<DC_RegActivityRequestEvalFilter>
            {
                Data = filter
            };
            var response = service.QueryCurrentRegActivityRequestEval(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult get(long feeNo, string regNo, string mark)
        {
            DC_RegActivityRequestEvalFilter filter = new DC_RegActivityRequestEvalFilter
            {
                FeeNo = feeNo,
                RegNo = regNo,
            };
            BaseRequest<DC_RegActivityRequestEvalFilter> request = new BaseRequest<DC_RegActivityRequestEvalFilter>
            {
                Data = filter
            };
            var response = service.QueryPartRegActivityRequestEval(request);
            return Ok(response);
        }

        [Route()]
        public IHttpActionResult post(DC_RegActivityRequestEval request)
        {
            try
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            catch
            {
                request.OrgId = "1";
            }
            service.saveRegActivityRequestEval(request);
            return Ok(request);
        }
    }

}

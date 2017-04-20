using KM.Common;
using KMHC.Infrastructure;
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
    [RoutePrefix("api/DCNurseRequirementEval")]
    public class DCNurseRequirementEvalController : BaseController
    {

        IDC_ResidentManageService service = IOCContainer.Instance.Resolve<IDC_ResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]int CurrentPage, int PageSize, long feeNo)
        {
            DcNurseingPlanEvalFilter filter = new DcNurseingPlanEvalFilter
            {
                FeeNo = feeNo
            };
            BaseRequest<DcNurseingPlanEvalFilter> request = new BaseRequest<DcNurseingPlanEvalFilter>
            {
                Data = filter
            };
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            var response = service.QueryDcNurseingPlanEval(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult get(long feeNo)
        {
            DcNurseingPlanEvalFilter filter = new DcNurseingPlanEvalFilter
            {
                FeeNo = feeNo
            };
            BaseRequest<DcNurseingPlanEvalFilter> request = new BaseRequest<DcNurseingPlanEvalFilter>
            {
                Data = filter
            };
            var response = service.QueryCurrentDcNurseingPlanEval(request);
            return Ok(response);
        }

        [Route()]
        public IHttpActionResult post(DcNurseingPlanEval request)
        {
            try
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            catch
            {
                request.OrgId = "1";
            }
            BaseResponse<DcNurseingPlanEval> response = service.saveNurseingPlanEval(request);
            return Ok(response);
        }
    }
}

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

    [RoutePrefix("api/DC_REGCPL")]
    public class DC_NursingPlanController : BaseController
    {
        IDC_ResidentManageService service = IOCContainer.Instance.Resolve<IDC_ResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult get(long feeNo, long Id, string mark)
        {
            DC_RegCplFilter filter = new DC_RegCplFilter
            {
                FeeNo = feeNo,
                Id = Id
            };
            BaseRequest<DC_RegCplFilter> request = new BaseRequest<DC_RegCplFilter>
            {
                Data = filter
            };
            var response = service.QueryCurrentRegCpl(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult get(long Id, long feeNo)
        {
            BaseResponse<string> response = new BaseResponse<string>
            {
                Data = service.QueryDCCplAction(Id, feeNo)
            };
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult get(string majorType)
        {
            var response = service.QueryLevelPr(majorType);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult get(string levelPr, string majorType)
        {

            var response = service.QueryDiaPr(levelPr, majorType);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult get(int? cpNo, string dirPr, string mark)
        {
            if (mark == "CpReason")
            {
                var response = service.QueryCausep(cpNo);
                return Ok(response);
            }
            else if (mark == "NsDesc")
            {
                var response = service.QueryPrData(cpNo);
                return Ok(response);
            }
            else if (mark == "Goal")
            {
                var response = service.QueryGoalp(dirPr);
                return Ok(response);
            }
            else if (mark == "Activity")
            {
                var response = service.QueryActivity(dirPr);
                return Ok(response);
            }
            else if (mark == "Assessvalue")
            {
                var response = service.QueryAssessvalue(dirPr);
                return Ok(response);
            }
            else
            {
                return Ok("");
            }

        }


        //[Route(""), HttpGet]
        //public IHttpActionResult get(string majorType)
        //{
        //    DC_CarePlanProblemFilter filter = new DC_CarePlanProblemFilter
        //    {
        //        MajorType = majorType
        //    };
        //    BaseRequest<DC_CarePlanProblemFilter> request = new BaseRequest<DC_CarePlanProblemFilter>
        //    {
        //        Data = filter
        //    };
        //    var response = service.QueryCarePlanProblem(request);
        //    return Ok(response);
        //}
        //[Route(""), HttpGet]
        //public IHttpActionResult get(int cpNo)
        //{
        //    DC_CarePlanDiaFilter filter = new DC_CarePlanDiaFilter
        //    {
        //        CpNo = cpNo
        //    };
        //    BaseRequest<DC_CarePlanDiaFilter> request = new BaseRequest<DC_CarePlanDiaFilter>
        //    {
        //        Data = filter
        //    };
        //    var response = service.QueryCarePlanDia(request);
        //    return Ok(response);
        //}
        //[Route(""), HttpGet]
        //public IHttpActionResult get(int cpNo, string mark)
        //{
        //    DC_CarePlanActivityFilter filter = new DC_CarePlanActivityFilter
        //    {
        //        CpNo = cpNo
        //    };
        //    BaseRequest<DC_CarePlanActivityFilter> request = new BaseRequest<DC_CarePlanActivityFilter>
        //    {
        //        Data = filter
        //    };
        //    var response = service.QueryCarePlanActivity(request);
        //    return Ok(response);
        //}
        [Route("")]
        public IHttpActionResult post(DC_RegCpl request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            request.CreateDate = DateTime.Now;
            var response = service.saveRegCpl(request);
            return Ok(response);
        }
    }
}

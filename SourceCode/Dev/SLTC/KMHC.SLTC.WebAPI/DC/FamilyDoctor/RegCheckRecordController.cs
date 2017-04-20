using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.FamilyDoctor
{
    [RoutePrefix("api/RegCheckRecord")]
    public class RegCheckRecordController : BaseController
    {
        IDC_FamilyDoctorService service = IOCContainer.Instance.Resolve<IDC_FamilyDoctorService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(DateTime? startDate, DateTime? endDate, string displayType, string traceStatus, string idNo, string regName, int currentPage, int pageSize)
        {
            DC_RegCheckRecordFilter filter = new DC_RegCheckRecordFilter
            {
                OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                StartDate = startDate,
                EndDate = endDate,
                DisplayType = displayType == "1",
                IdNo = idNo,
                RegName = regName
            };
            if(traceStatus != "0")
            {
                filter.TraceStatus = traceStatus == "2";
            }
            BaseRequest<DC_RegCheckRecordFilter> request = new BaseRequest<DC_RegCheckRecordFilter>
            {
                Data = filter
            };
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            var regCheckRecordList = service.QueryRegCheckRecord(request);
            return Ok(regCheckRecordList);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(string regNo, DateTime? startDate, DateTime? endDate, string displayType, string checkTemplateCode, int currentPage, int pageSize)
        {
            BaseRequest<DC_RegCheckRecordFilter> request = new BaseRequest<DC_RegCheckRecordFilter>();
            request.Data.StartDate = startDate;
            request.Data.EndDate = endDate;
            request.Data.DisplayType = displayType == "1";
            request.Data.CheckTemplateCode = checkTemplateCode;
            request.Data.RegNo = regNo;
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            var regCheckRecordList = service.GetRegCheckRecordDtl(request);
            return Ok(regCheckRecordList);
        }

        [Route(""), HttpPost]
        public IHttpActionResult Save(DC_RegCheckRecordModel request)
        {
            var response = service.SaveRegCheckRecord(request, new List<string>() { "TraceStatus" });
            return Ok(response);
        }
    }
}

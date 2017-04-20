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
    [RoutePrefix("api/RegNoteRecord")]
    public class RegNoteRecordController : BaseController
    {
        IDC_FamilyDoctorService service = IOCContainer.Instance.Resolve<IDC_FamilyDoctorService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(DateTime? startDate, DateTime? endDate, int currentPage, int pageSize)
        {
            var filter = new DC_RegNoteRecordFilter
            {
                OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                StartDate = startDate,
                EndDate = endDate
            };
            BaseRequest<DC_RegNoteRecordFilter> request = new BaseRequest<DC_RegNoteRecordFilter>
            {
                Data = filter
            };
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            var list = service.QueryRegNoteRecord(request);
            return Ok(list);
        }

        [Route(""), HttpPost]
        public IHttpActionResult Save(DC_RegNoteRecordModel request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveRegNoteRecord(request, null);
            return Ok(response);
        }

        [Route("{recordId}"), HttpPost]
        public IHttpActionResult SaveViewInfo(long recordId, DC_RegNoteRecordModel request)
        {
            request.RecordId = recordId;
            request.ViewStatus = 1;
            request.ViewDate = DateTime.Now;
            var response = service.SaveRegNoteRecord(request, new List<string>() { "ViewStatus", "ViewDate" });
            return Ok(response);
        }

        [Route("{recordId}")]
        public IHttpActionResult Delete(long recordId)
        {
            var response = service.DeleteRegNoteRecord(recordId);
            return Ok(response);
        }
    }
}

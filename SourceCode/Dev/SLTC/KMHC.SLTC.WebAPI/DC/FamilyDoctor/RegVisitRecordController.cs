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
    [RoutePrefix("api/RegVisitRecord")]
    public class RegVisitRecordController : BaseController
    {
        IDC_FamilyDoctorService service = IOCContainer.Instance.Resolve<IDC_FamilyDoctorService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(DateTime? startDate, DateTime? endDate, int currentPage, int pageSize)
        {
            var filter = new DC_RegVisitRecordFilter
            {
                OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                StartDate = startDate,
                EndDate = endDate
            };
            BaseRequest<DC_RegVisitRecordFilter> request = new BaseRequest<DC_RegVisitRecordFilter>
            {
                Data = filter
            };
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            var list = service.QueryRegVisitRecord(request);
            return Ok(list);
        }

        [Route(""), HttpPost]
        public IHttpActionResult Save(DC_RegVisitRecordModel request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveRegVisitRecord(request, null);
            return Ok(response);
        }

        [Route("{recordId}")]
        public IHttpActionResult Delete(long recordId)
        {
            var response = service.DeleteRegVisitRecord(recordId);
            return Ok(response);
        }
    }
}

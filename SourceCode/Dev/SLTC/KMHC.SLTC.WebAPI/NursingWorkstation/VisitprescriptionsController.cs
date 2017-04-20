using KM.Common;
using KMHC.Infrastructure;
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

namespace KMHC.SLTC.WebAPI.NursingWorkstation
{
    [RoutePrefix("visitprescriptions")]
    public class VisitprescriptionsController : BaseController
    {
        INursingManageService service = IOCContainer.Instance.Resolve<INursingManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]int seqNo)
        {
            VisitPrescriptionFilter filter = new VisitPrescriptionFilter
            {
                SeqNo = seqNo
            };
            BaseRequest<VisitPrescriptionFilter> request = new BaseRequest<VisitPrescriptionFilter>
            {
                Data = filter
            };
            var response = service.QueryVisitPreData(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(VisitPrescription baseRequest)
        {
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveVisitPreData(baseRequest);
            return Ok(response);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteVisitPreData(id);
            return Ok(response);
        }
    }
}

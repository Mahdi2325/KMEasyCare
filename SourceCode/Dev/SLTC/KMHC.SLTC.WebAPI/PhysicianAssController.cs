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

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/doctorEvalRec")]
    public class PhysicianAssController : BaseController
    {
        INursingWorkstationService service = IOCContainer.Instance.Resolve<INursingWorkstationService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get([FromUri]int CurrentPage, int PageSize, long feeNo)
        {
            DoctorEvalRecFilter filter = new DoctorEvalRecFilter
            {
                FeeNo = feeNo
            };
            BaseRequest<DoctorEvalRecFilter> request = new BaseRequest<DoctorEvalRecFilter>
            {
                Data = filter
            };
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            var doctorEvalRecList = service.QueryDocEvalRecData(request);
            return Ok(doctorEvalRecList);
        }
        [Route("")]
        public IHttpActionResult post(DoctorEvalRec request)
        {
            request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            request.CreateDate = DateTime.Now;
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveDocEvalRecData(request);
            return Ok(response.Data);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteDocEvalRecData(id);
            return Ok(response);
        }
    }
}

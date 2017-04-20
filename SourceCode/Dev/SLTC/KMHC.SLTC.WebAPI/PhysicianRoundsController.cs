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
    [RoutePrefix("api/doctorCheckRec")]
    public class PhysicianRoundsController : BaseController
    {
        INursingWorkstationService service = IOCContainer.Instance.Resolve<INursingWorkstationService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get([FromUri]int CurrentPage, int PageSize, long feeNo)
        {
            DoctorCheckRecFilter filter = new DoctorCheckRecFilter
            {
                FeeNo = feeNo
            };
            BaseRequest<DoctorCheckRecFilter> request = new BaseRequest<DoctorCheckRecFilter>
            {
                Data = filter
            };
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            var DocCheckRecList = service.QueryDocCheckRecData(request);
            return Ok(DocCheckRecList);
        }
        [Route("")]
        public IHttpActionResult post(DoctorCheckRec request)
        {
            request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            request.CreateDate = DateTime.Now;
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveDocCheckRecData(request);
            return Ok(response.Data);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteDocCheckRecData(id);
            return Ok(response);
        }
    }
}

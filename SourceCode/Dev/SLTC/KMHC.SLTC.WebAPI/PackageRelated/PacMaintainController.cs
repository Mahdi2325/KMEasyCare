using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.PackageRelated;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.PackageRelated
{
    [RoutePrefix("api/PacMaintain")]
    public class PacMaintainController : BaseController
    {
        IPackageRelatedService service = IOCContainer.Instance.Resolve<IPackageRelatedService>();
        [Route("")]
        public IHttpActionResult Post(CHARGEGROUP baseRequest)
        {
            baseRequest.NSID = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SavePacMaintain(baseRequest);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage = 1, int pageSize = 10, string chargegroupName = "")
        {
            BaseRequest<PackageRelatedFilter> request = new BaseRequest<PackageRelatedFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { Name = chargegroupName }
            };
            var response = service.QueryChargeGroupList(request);
            return Ok(response);
        }
        [Route("{id}"), HttpGet]
        public IHttpActionResult Get(string id)
        {
            var response = service.GetChargeGroup(id);
            return Ok(response);
        }
        [Route("{id}"),HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            var response = service.DeleteChargeGroup(id);
            return Ok(response);
        }
        [Route("DeleteChargeItem/{id}"), HttpDelete]
        public IHttpActionResult DeleteChargeItem(int id)
        {
            var response = service.DeleteChargeItem(id);
            return Ok(response);
        }
    }
}

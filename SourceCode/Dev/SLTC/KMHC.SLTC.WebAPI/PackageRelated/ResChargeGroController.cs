using KM.Common;
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
    [RoutePrefix("api/ResChargeGro")]
    public class ResChargeGroController : BaseController
    {
        IPackageRelatedService service = IOCContainer.Instance.Resolve<IPackageRelatedService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get(int feeNo, int CurrentPage, int PageSize)
        {
            BaseRequest<PackageRelatedFilter> request = new BaseRequest<PackageRelatedFilter>();
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            request.Data.FeeNO = feeNo;
            var response = service.QueryResChargeGro(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(RESCHARGEGRO baseRequest)
        {
            var response = service.SaveResChargeGro(baseRequest);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteResChargeGro(id);
            return Ok(response);
        }
    }
}

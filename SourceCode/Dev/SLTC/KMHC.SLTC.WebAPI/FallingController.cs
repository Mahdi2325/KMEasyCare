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
    [RoutePrefix("api/fallincidentevent")]
    public class FallingController : BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]int CurrentPage, int PageSize, long feeNo)
        {
            FallIncidentEventFilter filter = new FallIncidentEventFilter
            {
                FeeNo = feeNo
            };
            BaseRequest<FallIncidentEventFilter> request = new BaseRequest<FallIncidentEventFilter>
            {
                Data = filter
            };
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            var response = service.QueryFallIncidentEvent(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(FallIncidentEvent request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveFallIncidentEvent(request);
            return Ok(response.Data);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteFallIncidentEvent(id);
            return Ok(response);
        }
    }
}

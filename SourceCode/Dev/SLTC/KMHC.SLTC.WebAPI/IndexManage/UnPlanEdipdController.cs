using KMHC.Infrastructure;

namespace KMHC.SLTC.WebAPI
{
    using KM.Common;
    using KMHC.SLTC.Business.Entity;
    using KMHC.SLTC.Business.Entity.Base;
    using KMHC.SLTC.Business.Entity.Filter;
    using KMHC.SLTC.Business.Entity.Model;
    using KMHC.SLTC.Business.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("api/UnPlanEdipd")]
    public class UnPlanWeightIndController : BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(long regNo = 0, long feeNo = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<UnPlanEdipdFilter> request = new BaseRequest<UnPlanEdipdFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.RegNo = regNo;
            request.Data.FeeNo = feeNo;
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.QueryUnPlanEdipd(request);
            return Ok(response);
        }

        public IHttpActionResult Query(long regNo = 0, long feeNo = 0)
        {
            BaseRequest<UnPlanEdipdFilter> request = new BaseRequest<UnPlanEdipdFilter>();
            request.Data.RegNo = regNo;
            request.Data.FeeNo = feeNo;
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.QueryNewUnPlanEdipd(request);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Get(long id)
        {
            var response = service.GetUnPlanEdipd(id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(UnPlanEdipd baseRequest)
        {
            var response = service.SaveUnPlanEdipd(baseRequest);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteUnPlanEdipd(id);
            return Ok(response);
        }
    }
}

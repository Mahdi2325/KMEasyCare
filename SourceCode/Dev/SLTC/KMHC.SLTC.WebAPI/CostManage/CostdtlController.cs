namespace KMHC.SLTC.WebAPI
{
    using KM.Common;
    using KMHC.Infrastructure;
    using KMHC.SLTC.Business.Entity;
    using KMHC.SLTC.Business.Entity.Base;
    using KMHC.SLTC.Business.Entity.Filter;
    using KMHC.SLTC.Business.Entity.Model;
    using KMHC.SLTC.Business.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("api/Costdtl")]
    public class CostDtlController : BaseController
    {
        ICostManageService service = IOCContainer.Instance.Resolve<ICostManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage=1, int pageSize=100, int BillId=0, int FeeNo=0,int OrgeId=0)
        {
            BaseRequest<CostDtlFilter> request = new BaseRequest<CostDtlFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FeeNo = FeeNo, BillId = BillId, OrgId = SecurityHelper.CurrentPrincipal.OrgId}
            };
            var response = service.QueryCostDtl(request);  
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Get(long id)
        {
            var response = service.GetCostDtl(id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(CostDtl baseRequest)
        {
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveCostDtl(baseRequest);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteCostDtl(id);
            return Ok(response);
        }
    }
}

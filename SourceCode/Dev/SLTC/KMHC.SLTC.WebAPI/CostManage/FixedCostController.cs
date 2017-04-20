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
    
    [RoutePrefix("api/FixedCost")]
    public class FixedcostController : BaseController
    {
        ICostManageService service = IOCContainer.Instance.Resolve<ICostManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage =1 ,int pageSize = 100,int feeno = 0)
        {
            var request = new BaseRequest<FixedCostFilter>()
            {
                Data = new FixedCostFilter()
                {
                    FeeNo = feeno,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId
                },
                CurrentPage = currentPage,
                PageSize = pageSize,

            };
            var response = service.QueryFixedCost(request);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetFixedCost(id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(FixedCost baseRequest)
        {
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveFixedCost(baseRequest);
            return Ok(response);
        }

        [Route("ChargeGroup"), HttpPost]
        public IHttpActionResult PostGroup(FixedCost request)
        {
            var response = service.SaveFixedCost(request.Id, request.RegNo, request.FeeNo,request.StartDate,request.EndDate);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteFixedCost(id);
            return Ok(response);
        }
    }
}

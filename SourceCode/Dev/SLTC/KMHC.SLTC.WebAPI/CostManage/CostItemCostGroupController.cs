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

    [RoutePrefix("api/CostItemCostGroup")]
    public class CostItemCostGroupController : BaseController
    {
        ICostManageService service = IOCContainer.Instance.Resolve<ICostManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage=1, int pageSize=100, string keyWords="", int feeNo = 0)
        {
            BaseRequest<CostItemCostGroupFilter> request = new BaseRequest<CostItemCostGroupFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.No = keyWords;
            request.Data.Name = keyWords;
            var response = service.QueryCostItemAndCostGroup(request);
            return Ok(response);
        }
    }
}

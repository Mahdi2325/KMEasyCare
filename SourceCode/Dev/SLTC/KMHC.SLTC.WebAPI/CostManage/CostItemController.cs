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
    
    [RoutePrefix("api/CostItem")]
    public class CostItemController : BaseController
    {
        ICostManageService service = IOCContainer.Instance.Resolve<ICostManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage=1, int pageSize=100, string keyWords="", int feeNo = 0)
        {
            BaseRequest<CostItemFilter> request = new BaseRequest<CostItemFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.CostItemNo = keyWords;
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.QueryCostItem(request);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetCostItem(id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(CostItem baseRequest)
        {
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveCostItem(baseRequest);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteCostItem(id);
            return Ok(response);
        }
    }
}

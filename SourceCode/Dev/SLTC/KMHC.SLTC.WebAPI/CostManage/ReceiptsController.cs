namespace KMHC.SLTC.WebAPI.CostManage
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

    [RoutePrefix("api/Receipts")]
    public class ReceiptsController : BaseController
    {
        ICostManageService service = IOCContainer.Instance.Resolve<ICostManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int FeeNo, string OrgId)
        {
            BaseRequest<ReceiptsFilter> request = new BaseRequest<ReceiptsFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FeeNo = FeeNo, OrgId = OrgId }
            };
            var response = service.QueryReceipts(request);
            return Ok(response);
        }

        [Route("{Id}")]
        public IHttpActionResult Get(long Id)
        {
            var response = service.GetReceipts(Id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(Receipts baseRequest)
        {
            var response = service.SaveReceipts(baseRequest);
            return Ok(response);
        }

        [Route("{Id}")]
        public IHttpActionResult Delete(long Id)
        {
            var response = service.DeleteReceipts(Id);
            return Ok(response);
        }

    }
}

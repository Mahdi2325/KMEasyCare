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
    
    [RoutePrefix("api/Bill")]
    public class BillController : BaseController
    {
        ICostManageService service = IOCContainer.Instance.Resolve<ICostManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int FeeNo, string OrgId)
        {
            BaseRequest<BillFilter> request = new BaseRequest<BillFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FeeNo = FeeNo, OrgId = OrgId }
            };
            var response = service.QueryBill(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(int FeeNo, string OrgId)
        {
            var response= service.GenerateBill(FeeNo,OrgId);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Get(long id)
        {
            var response = service.GetBill(id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(Bill baseRequest)
        {
            var response = service.SaveBill(baseRequest);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteBill(id);
            return Ok(response);
        }
    }
}

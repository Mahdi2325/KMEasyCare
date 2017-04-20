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
    
    [RoutePrefix("api/PayBill")]
    public class PayBillController : BaseController
    {
        ICostManageService service = IOCContainer.Instance.Resolve<ICostManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int BillId)
        {
            BaseRequest<PayBillFilter> request = new BaseRequest<PayBillFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { BillId = BillId }
            };
            var response = service.QueryPayBill(request);  
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(int key)
        {
            BaseRequest<PayBillFilter> request = new BaseRequest<PayBillFilter>
            {
                Data = { BillId = key }
            };
            var response = service.getSum(request);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Get(long id)
        {
            var response = service.GetPayBill(id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(PayBill baseRequest)
        {

            var response = service.SavePayBill(baseRequest);
            
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeletePaybill(id);
            return Ok(response);
        }
    }
}

using KM.Common;
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

    [RoutePrefix("api/billPaymentRec")]
    public class PaymentRecController : BaseController
    {
        IPaymentRecService service = IOCContainer.Instance.Resolve<IPaymentRecService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int FeeNo, int Status)
        {
            BaseRequest<PaymentMgmtFilter> request = new BaseRequest<PaymentMgmtFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FEENO = FeeNo, STATUS = Status }
            };
            var response = service.QueryBillV2(request);
            return Ok(response);
        }

        /// <summary>
        /// 获取账单明细
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, string billid)
        {
            BaseRequest<PaymentMgmtFilter> request = new BaseRequest<PaymentMgmtFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { BILLID = billid }
            };
            var response = service.QueryFeeRecord(request);
            return Ok(response);
        }

        /// <summary>
        /// 获取缴费账单记录
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int billPayId, string billCharge)
        {
            BaseRequest<PaymentMgmtFilter> request = new BaseRequest<PaymentMgmtFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { BILLPAYID = billPayId }
            };
            var response = service.QueryBillV2PAY(request);
            return Ok(response);
        }
    }
}

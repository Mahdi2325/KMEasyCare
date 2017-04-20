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

    [RoutePrefix("api/billRefundRec")]
    public class RefundRecController : BaseController
    {
        IRefundRecService service = IOCContainer.Instance.Resolve<IRefundRecService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int FeeNo, int Status)
        {
            BaseRequest<RefundMgmtFilter> request = new BaseRequest<RefundMgmtFilter>
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
        public IHttpActionResult Query(int currentPage, int pageSize, string billId)
        {
            BaseRequest<RefundMgmtFilter> request = new BaseRequest<RefundMgmtFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { BILLID = billId }
            };
            var response = service.QueryFeeRecord(request);
            return Ok(response);
        }

        /// <summary>
        /// 获取缴费账单记录
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int RefundRecordId)
        {
            BaseRequest<RefundMgmtFilter> request = new BaseRequest<RefundMgmtFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { REFUNDRECORDID = RefundRecordId }
            };
            var response = service.QueryRefund(request);
            return Ok(response);
        }
    }
}

using KM.Common;
namespace KMHC.SLTC.WebAPI
{
    using Infrastructure;
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

    [RoutePrefix("api/billRefund")]
    public class RefundMgmtController : BaseController
    {
        IRefundMgmtService service = IOCContainer.Instance.Resolve<IRefundMgmtService>();

        public string UpdateBy { get; private set; }
        public DateTime UpdateDate { get; private set; }

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
        /// 获取收费明细
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
        /// 获取住民账户信息
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        public IHttpActionResult QueryResidentBalance(int FeeNo)
        {
            BaseRequest<PaymentMgmtFilter> request = new BaseRequest<PaymentMgmtFilter>
            {
                Data = { FEENO = FeeNo }
            };
            var response = service.QueryRESIDENTBALANCE(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult SaveBillV2Info(BillV2List baseRequest)
        {
            var response = service.SaveBillV2Info(baseRequest);
            return Ok(response);
        }
    }
}

using KM.Common;
namespace KMHC.SLTC.WebAPI
{
    using Business.Entity.Model.FinancialManagement;
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

    [RoutePrefix("api/advanceChargeRefund")]
    public class AdvanceChargeRefundController : BaseController
    {
        IAdvanceChargeRefundService service = IOCContainer.Instance.Resolve<IAdvanceChargeRefundService>();

        public string CreateBy { get; private set; }
        public DateTime CreateDate { get; private set; }

        [Route(""), HttpGet]
        public IHttpActionResult QueryResidentBalanceRefund(int currentPage, int pageSize, int FeeNo)
        {
            BaseRequest<PaymentMgmtFilter> request = new BaseRequest<PaymentMgmtFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FEENO = FeeNo }
            };
            var response = service.QueryResidentBalanceRefund(request);
            return Ok(response);
        }


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
        public IHttpActionResult SaveResidentBalance(ResidentBalance baseRequest, decimal Balance)
        {
            var response = service.SaveResidentBalance(baseRequest, Balance);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult SaveResidentBalanceRefund(ResidentBalanceRefund baseRequest)
        {
            CreateBy = SecurityHelper.CurrentPrincipal.LoginName;
            CreateDate = DateTime.Now;
            var response = service.SaveResidentBalanceRefund(baseRequest, CreateBy, CreateDate);
            return Ok(response);
        }
    }
}

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

    [RoutePrefix("api/advanceCharge")]
    public class AdvanceChargeController : BaseController
    {
        IAdvanceChargeService service = IOCContainer.Instance.Resolve<IAdvanceChargeService>();

        public string CreateBy { get; private set; }
        public DateTime CreateDate { get; private set; }

        [Route(""), HttpGet]
        public IHttpActionResult QueryPreCharge(int currentPage, int pageSize, int FeeNo)
        {
            BaseRequest<PaymentMgmtFilter> request = new BaseRequest<PaymentMgmtFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FEENO = FeeNo }
            };
            var response = service.QueryPreCharge(request);
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
        public IHttpActionResult SavePreCharge(PreCharge baseRequest)
        {
            CreateBy = SecurityHelper.CurrentPrincipal.LoginName;
            CreateDate = DateTime.Now;
            var response = service.SavePreCharge(baseRequest, CreateBy, CreateDate);
            return Ok(response);
        }
    }
}

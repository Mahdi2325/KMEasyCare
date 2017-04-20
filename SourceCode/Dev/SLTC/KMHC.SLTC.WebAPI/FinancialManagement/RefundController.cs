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

    [RoutePrefix("api/RefundRes")]
    public class RefundController : BaseController
    {
        IRefundService service = IOCContainer.Instance.Resolve<IRefundService>();

        public string CreateBy { get; private set; }
        public DateTime CreateDate { get; private set; }

        [Route("")]
        public IHttpActionResult SaveRefund(Refund baseRequest)
        {
            CreateBy = SecurityHelper.CurrentPrincipal.LoginName;
            CreateDate = DateTime.Now;
            var response = service.SaveRefund(baseRequest, CreateBy, CreateDate);
            return Ok(response);
        }
    }
}

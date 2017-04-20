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

    [RoutePrefix("api/billV2PayRes")]
    public class BillV2PayController : BaseController
    {
        IBillV2PayService service = IOCContainer.Instance.Resolve<IBillV2PayService>();

        public string CreateBy { get; private set; }
        public DateTime CreateDate { get; private set; }

        [Route("")]
        public IHttpActionResult SaveBillV2Pay(BillV2PAY baseRequest)
        {
            CreateBy = SecurityHelper.CurrentPrincipal.LoginName;
            CreateDate = DateTime.Now;
            var response = service.SaveBillV2Pay(baseRequest, CreateBy, CreateDate);
            return Ok(response);
        }
    }
}

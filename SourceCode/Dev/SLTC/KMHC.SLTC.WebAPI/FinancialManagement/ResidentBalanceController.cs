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

    [RoutePrefix("api/residentBalanceRes")]
    public class ResidentBalanceController : BaseController
    {
        IResidentBalanceService service = IOCContainer.Instance.Resolve<IResidentBalanceService>();

        public string UpdateBy { get; private set; }
        public DateTime UpdateDate { get; private set; }

        [Route("")]
        public IHttpActionResult SaveResidentBalance(ResidentBalance baseRequest)
        {
            UpdateBy = SecurityHelper.CurrentPrincipal.LoginName;
            UpdateDate = DateTime.Now;
            var response = service.SaveResidentBalance(baseRequest, UpdateBy, UpdateDate);
            return Ok(response);
        }
    }
}

namespace KMHC.SLTC.WebAPI.CostManage
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

    [RoutePrefix("api/PinMoney")]
    public class PinMoneyController : BaseController
    {
        ICostManageService service = IOCContainer.Instance.Resolve<ICostManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int FeeNo, string OrgId)
        {
            BaseRequest<PinMoneyFilter> request = new BaseRequest<PinMoneyFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FeeNo = FeeNo, OrgId = OrgId }
            };
            var response = service.QueryPinMoney(request);
            return Ok(response);
        }

        [Route("{Id}")]
        public IHttpActionResult Get(long Id)
        {
            var response = service.GetPinMoney(Id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(PinMoney baseRequest)
        {
            var response = service.SavePinMoney(baseRequest);
            return Ok(response);
        }

        [Route("{Id}")]
        public IHttpActionResult Delete(long Id)
        {
            var response = service.DeletePinMoney(Id);
            return Ok(response);
        }

    }
}

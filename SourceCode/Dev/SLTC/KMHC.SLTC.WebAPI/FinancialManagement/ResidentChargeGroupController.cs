using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.FinancialManagement
{
    [RoutePrefix("api/rsChargeGroup")]
    public class ResidentChargeGroupController : BaseController
    {
        IRsChargeGroupService service = IOCContainer.Instance.Resolve<IRsChargeGroupService>();
        /// <summary>
        /// 获取住民绑定套餐明细/分页
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        public IHttpActionResult QueryRsChargeGroup(int CurrentPage, int PageSize, int FeeNo)
        {
            BaseRequest<PaymentMgmtFilter> request = new BaseRequest<PaymentMgmtFilter>
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = { FEENO = FeeNo }
            };
            var response = service.QueryRsChargeGroup(request);
            return Ok(response);
        }

        /// <summary>
        /// 获取住民绑定套餐总明细
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        public IHttpActionResult GetRsChargeGroup(int FeeNo, string Type)
        {
            BaseRequest<PaymentMgmtFilter> request = new BaseRequest<PaymentMgmtFilter>
            {
                Data = { FEENO = FeeNo }
            };
            var response = service.GetRsChargeGroup(request);
            return Ok(response);
        }
    }
}

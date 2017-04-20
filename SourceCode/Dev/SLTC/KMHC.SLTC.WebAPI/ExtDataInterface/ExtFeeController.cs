using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.Filter;
using KM.Common;
using KMHC.Infrastructure;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.ChargeInputModel;

namespace KMHC.SLTC.WebAPI.ExtDataInterface
{
    //LTC数据对外查询接口
    [RoutePrefix("api/Ext/Fee")]
    public class ExtFeeController : BaseController
    {
        private readonly ICostService _costService = IOCContainer.Instance.Resolve<ICostService>();
        [Route("GetFeeRecords"), HttpGet]
        public IHttpActionResult GetFeeRecords(int feeNo, DateTime st, DateTime et)
        {
            var response = _costService.QueryCommonRec(new BaseRequest<ServiceRecordFilter>()
            {
                PageSize = 999999,
                CurrentPage = 1,
                Data = new ServiceRecordFilter()
            {
                FeeNo = feeNo
            }
            });

            //TODO, 改进转义方式
            foreach (var fee in response.Data)
            {
                if (fee.RecordType=="S")
                {
                    fee.RecordType = "服务";
                }
                else if (fee.RecordType == "M")
                {
                    fee.RecordType = "耗材";
                }
                else if (fee.RecordType == "D")
                {
                    fee.RecordType = "药品";
                }
            }
            return Ok(response);
        }
    }
}

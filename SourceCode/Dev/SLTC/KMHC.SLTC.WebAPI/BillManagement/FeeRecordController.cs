using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.BillManagement
{
    [RoutePrefix("api/feeRecord")]
    public class FeeRecordController : BaseController
    {
        IFeeRecordService service = IOCContainer.Instance.Resolve<IFeeRecordService>(); 

        [Route(""), HttpGet]
        public IHttpActionResult Query(int FeeNo, DateTime sDate, DateTime eDate, string taskStatus)
        {
            BaseRequest<FeeRecordFilter> request = new BaseRequest<FeeRecordFilter>
            {
                Data = { FeeNo = FeeNo, SDate = sDate, EDate = eDate.AddDays(1).AddSeconds(-1), TaskStatus = taskStatus }
            };
            
            var response = service.QueryNotGenerateBillRecord(request);
            return Ok(response);
        }

        [Route(""), HttpPost]
        public IHttpActionResult post(BillV2Info request)
        {
            var response = service.SaveFeeRecord(request);
            return Ok(response);
        }

    }
}

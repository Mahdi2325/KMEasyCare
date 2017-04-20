#region 文件描述
/******************************************************************
** 创建人   :BobDu
** 创建时间 :2017/3/30 
** 说明     :
******************************************************************/
#endregion

using KM.Common;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DictManage
{
    [RoutePrefix("api/DictManage")]
    public class DictManageController : BaseController
    {
        IDictManageService _service = IOCContainer.Instance.Resolve<IDictManageService>();
        [Route("GetFeeIntervalByMonth/{month}"), HttpGet]
        public IHttpActionResult GetFeeIntervalByMonth(string month)
        {
            var response = _service.GetFeeIntervalByMonth(month);
            return Ok(response);
        }
        [Route("GetFeeIntervalByDateStr/{date}"), HttpGet]
        public IHttpActionResult GetFeeIntervalByDateStr(string date)
        {
            var response = _service.GetFeeIntervalByDate(date);
            return Ok(response);
        }
        [Route("GetFeeIntervalByDateTime/{d}"), HttpGet]
        public IHttpActionResult GetFeeIntervalByDateTime(DateTime d)
        {
            var response = _service.GetFeeIntervalByDate(d);
            return Ok(response);
        }
        [Route("GetFeeIntervalByYearMonth/{yearMonth}"), HttpGet]
        public IHttpActionResult GetFeeIntervalByYearMonth(string yearMonth)
        {
            var response = _service.GetFeeIntervalByYearMonth(yearMonth);
            return Ok(response);
        }
    }
}

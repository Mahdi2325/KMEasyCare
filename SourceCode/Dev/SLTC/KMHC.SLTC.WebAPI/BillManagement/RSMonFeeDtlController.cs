using KMHC.Infrastructure;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.BillManagement
{
    [RoutePrefix("api/rsFeeMonDtl")]
    public class RSMonFeeDtlController : BaseController
    {
        [Route(""), HttpGet]
        public async Task<object> QueryRSMonFee(int currentPage,int pageSize,int orgMonFeeId)
        {
            var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/rsMonFeeDtl?currentPage=" + currentPage + "&pageSize=" + pageSize + "&orgMonFeeId=" + orgMonFeeId);
            object resultContent = await result.Content.ReadAsAsync<object>();
            return resultContent;
        }

        [Route(""), HttpGet]
        public async Task<object> QueryRSMonFeeDtl(int currentPage, int pageSize, int orgMonFeeId, string feeType)
        {
            var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/rsMonFeeDtl?currentPage=" + currentPage + "&pageSize=" + pageSize + "&orgMonFeeId=" + orgMonFeeId + "&feeType=" + feeType);
            object resultContent = await result.Content.ReadAsAsync<object>();
            return resultContent;
        }
    }
}

using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using Newtonsoft.Json;
using KMHC.SLTC.Business.Entity.Filter;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/FinancialClose")]
    public class FinancialCloseController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();
        
        [Route(""), HttpGet]
        public IHttpActionResult Query(long feeNo, DateTime financialCloseTime, string type)
        {
            var response = service.CloseOrCnacleBill(feeNo, financialCloseTime, type);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public async Task<object> CancelMonthData(string yearMonth)
        {
            var http = HttpClientHelper.LtcHttpClient;
            var request = new BillV2();
            request.BillMonth = yearMonth;
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId ;
            var monFeeInfo = new MonFeeModel();
            try
            {
                var result = await http.PostAsJsonAsync("/api/monFee/GetNsMonfee", request);
                var resultContent = await result.Content.ReadAsStringAsync();
                monFeeInfo = JsonConvert.DeserializeObject<MonFeeModel>(resultContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return monFeeInfo;
        }

        [Route("")]
        public IHttpActionResult Post(CloseFinancialFilter request)
        {
            var response = service.SaveIpdregInfo(request.type, request.financialCloseTime,request.feeNo);
            return Ok(response);
        }
    }
}

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
    [RoutePrefix("api/appropriation")]
    public class PayGrantController : BaseController
    {
        IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        [Route(""), HttpGet]
        public async Task<object> Query(string year)
        {
            string nsno = service.QueryOrgnsno(SecurityHelper.CurrentPrincipal.OrgId);
            int type = 0;
            var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/payGrant?year=" + year + "&nsno=" + nsno + "&type=" + type);
            object resultContent = await result.Content.ReadAsAsync<object>();
            return resultContent;
        }

        [Route(""), HttpGet]
        public async Task<object> Query(int grantId, int type)
        {
            var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/payGrant?grantId=" + grantId + "&type=" + type);
            object resultContent = await result.Content.ReadAsAsync<object>();
            return resultContent;
        }
    }
}

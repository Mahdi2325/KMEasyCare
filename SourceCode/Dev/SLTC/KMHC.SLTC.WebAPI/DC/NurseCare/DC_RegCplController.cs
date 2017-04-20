using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.NurseCare
{
      [RoutePrefix("api/DCRegCPL")]
    public class DC_RegCplController:BaseController
    {

        IDC_ResidentManageService service = IOCContainer.Instance.Resolve<IDC_ResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult get(long feeNo)
        {
            DC_RegCplFilter filter = new DC_RegCplFilter
            {
                FeeNo = feeNo
            };
            BaseRequest<DC_RegCplFilter> request = new BaseRequest<DC_RegCplFilter>
            {
                Data = filter
            };
            var response = service.QueryRegCpl(request);
            return Ok(response);
        }

    }
}

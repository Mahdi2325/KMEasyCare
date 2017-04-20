using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.ResidentManage
{
    [RoutePrefix("api/DCResidentRes")]
    public class DC_ResidentController : BaseController
    {
        IDC_ResidentManageService service = IOCContainer.Instance.Resolve<IDC_ResidentManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(string residentName, string ipdFlag,string stationCode, string residentNo)
        {
            BaseRequest<DC_ResidentFilter> request = new BaseRequest<DC_ResidentFilter>();
 

            request.Data.ResidentName = residentName;
            request.Data.IpdFlag = ipdFlag;
            request.Data.StationCode = stationCode;
            request.Data.ResidentNo = residentNo;
            var response = service.QueryDCResident(request);
            return Ok(response);
        }

        [Route("{regNo}"), HttpGet]
        public IHttpActionResult Get(string regNo)
        {
            var response = service.GetDCResident(regNo);
            return Ok(response);
        }
    }
}

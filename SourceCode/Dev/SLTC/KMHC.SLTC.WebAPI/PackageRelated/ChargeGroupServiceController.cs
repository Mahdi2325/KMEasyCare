using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.ChargeInputModel;
using KMHC.SLTC.Business.Entity.PackageRelated;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.PackageRelated
{

    [RoutePrefix("api/chargeGroupService")]
    public class ChargeGroupServiceController : BaseController
    {
        IServiceRecordService service = IOCContainer.Instance.Resolve<IServiceRecordService>();

        [Route("")]
        public IHttpActionResult SaveServiceRecord(CHARGEITEM baseRequest)
        {
            var response = service.SaveServiceRecord(baseRequest);
            return Ok(response);
        }
    }
}

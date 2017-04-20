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

    [RoutePrefix("api/chargeGroupDrug")]
    public class ChargeGroupDrugController : BaseController
    {
        IDrugRecordService service = IOCContainer.Instance.Resolve<IDrugRecordService>();

        [Route("")]
        public IHttpActionResult SaveDrugRecord(CHARGEITEM baseRequest)
        {
            var response = service.SaveDrugRecord(baseRequest);
            return Ok(response);
        }
    }
}

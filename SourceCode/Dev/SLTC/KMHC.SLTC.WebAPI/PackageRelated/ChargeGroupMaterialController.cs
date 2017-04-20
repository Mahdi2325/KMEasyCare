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

    [RoutePrefix("api/chargeGroupMaterial")]
    public class ChargeGroupMaterialController : BaseController
    {
        IMaterialRecordService service = IOCContainer.Instance.Resolve<IMaterialRecordService>();

        [Route("")]
        public IHttpActionResult SaveMaterialRecord(CHARGEITEM baseRequest)
        {
            var response = service.SaveMaterialRecord(baseRequest);
            return Ok(response);
        }
    }
}

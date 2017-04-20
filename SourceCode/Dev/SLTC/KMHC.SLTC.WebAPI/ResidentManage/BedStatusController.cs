using KM.Common;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.ResidentManage
{
    [RoutePrefix("api/bedStatusRes")]
    public class BedStatusController : BaseController
    {
        IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        [Route("")]
        public IHttpActionResult Post([FromBody]BedBasic baseRequest)
        {
            var response = organizationManageService.UpdateBedBasic(baseRequest);
            return Ok(response);
        }
    }
}

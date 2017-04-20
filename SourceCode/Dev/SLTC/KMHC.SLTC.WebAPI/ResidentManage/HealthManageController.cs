using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
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
    [RoutePrefix("api/healthManageRes")]
    public class HealthManageController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(BaseRequest<HealthFilter> request)
        {
            var response = service.QueryHealth(request);
            return Ok(response.Data);
        }

        [Route("")]
        public IHttpActionResult Get([FromUri]long feeNo)
        {
            var response = service.GetHealth(feeNo);
            return Ok(response);
        }

     

        [Route("")]
        public IHttpActionResult Post(Health request)
        {
            request.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveHealth(request);
            return Ok(response);
        }

        [Route("{Id}")]
        public IHttpActionResult Delete(long feeNo)
        {
            var response = service.DeleteHealth(feeNo);
            return Ok(response);
        }
    }
}

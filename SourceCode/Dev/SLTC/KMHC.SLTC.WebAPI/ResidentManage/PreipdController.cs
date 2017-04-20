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

    [RoutePrefix("api/Preipd")]
    public class PreipdController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get([FromUri]BaseRequest<PreipdFilter> request)
        {
            var response = service.QueryPreipd(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(Preipd request)
        {
            request.UpdateDate = DateTime.Now;
            try {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            catch(Exception ex)
            {
                request.OrgId ="1";
            }

            var response = service.SavePreipd(request);
            return Ok(response.Data);
        }

        [Route("")]
        public IHttpActionResult Delete([FromUri]long recId)
        {
            var response = service.DeletePreipd(recId);
            return Ok(response);
        }

    }
}

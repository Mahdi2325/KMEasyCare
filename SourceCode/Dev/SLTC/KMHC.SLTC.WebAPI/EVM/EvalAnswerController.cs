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

namespace KMHC.SLTC.WebAPI.EVM
{
    [RoutePrefix("api/evalAnswer")]
    public class EvalAnswerController : BaseController
    {
        IOrganizationManageService Service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        [Route("")]
        public IHttpActionResult Post(LTC_MakerItemLimitedValue request)
        {
            var response = Service.SaveAnswer(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Delete(int id)
        {
            var response = Service.DeleteAnswer(id);
            return Ok(response);
        }
    }
}

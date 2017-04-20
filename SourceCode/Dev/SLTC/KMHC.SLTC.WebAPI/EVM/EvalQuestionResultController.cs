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
    [RoutePrefix("api/evalQuestionResult")]
    public class EvalQuestionResultController : BaseController
    {
        IOrganizationManageService Service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        [Route("")]
        public IHttpActionResult Post(LTC_QuestionResults request)
        {
            var response = Service.SaveQuestionResults(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Delete(int id)
        {
            var response = Service.DeleteQuestionResults(id);
            return Ok(response);
        }
    }
}

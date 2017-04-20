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
    [RoutePrefix("api/exportEval")]
    public class ExportEvalController : BaseController
    {
        IOrganizationManageService Service = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        [Route("")]
        public IHttpActionResult Post(LTC_Question request)
        {
            if (string.IsNullOrEmpty(request.OrgId))
            { request.OrgId = SecurityHelper.CurrentPrincipal.OrgId; }
            var response = Service.SaveQuestionModleData(request.QuestionId??0, request.OrgId,request.ExportQuestionId??0);
            return Ok(response);
        }


    }
}

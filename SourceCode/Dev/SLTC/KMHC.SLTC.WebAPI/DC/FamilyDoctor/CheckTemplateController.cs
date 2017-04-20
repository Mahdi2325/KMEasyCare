using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.FamilyDoctor
{
    [RoutePrefix("api/CheckTemplate")]
    public class CheckTemplateController : BaseController
    {
        IDC_FamilyDoctorService service = IOCContainer.Instance.Resolve<IDC_FamilyDoctorService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get()
        {
            BaseRequest<CheckTemplateFilter> request = new BaseRequest<CheckTemplateFilter>();
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            //request.Data.OrgId = "0000000001";
            request.CurrentPage = 1;
            request.PageSize = 100;
            var checkTemplateList = service.GetCheckTemplateList(request);
            return Ok(checkTemplateList);
        }
    }
}

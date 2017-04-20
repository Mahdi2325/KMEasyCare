using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.OrganizationManage
{
    [RoutePrefix("api/empFileExt")]
    public class EmpFileExtController : BaseController
    {
        IOrganizationManageService empFileService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(string empName = "", string empGroup = "", int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<EmployeeFilter> request = new BaseRequest<EmployeeFilter>()
            {
                CurrentPage = currentPage,
                PageSize = pageSize,

                Data =
                {
                    EmpName = empName,
                    EmpGroup = empGroup,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId
                }
            };
            var response = empFileService.QueryEmployeeExt(request);
            return Ok(response);
        }
    }
}

#region 文件描述
/******************************************************************
** 创建人   :BobDu
** 创建时间 :
** 说明     :
******************************************************************/
#endregion

using KM.Common;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.OrganizationManage
{
    [RoutePrefix("api/Ipd")]
    public class IpdController : BaseController
    {
        IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get(string nsno)
        {
            var res = organizationManageService.GetIpdByNsno(nsno);
            return Ok(res);
        }
    }
}

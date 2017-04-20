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
    [RoutePrefix("api/empfilesgetprosn")]
    public class RegFileGetcontroller : BaseController
    {

        readonly INursingRecord _INursingRecord = IOCContainer.Instance.Resolve<INursingRecord>();

        /// <summary>
        /// 获取人员基本信息
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get([FromUri]int regon)
        {
            var response = _INursingRecord.GetPr(regon);

            return Ok(response);

        }


    }
}


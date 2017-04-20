using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.Filter;
using KM.Common;
using KMHC.Infrastructure;
using System.Threading.Tasks;

namespace KMHC.SLTC.WebAPI.ExtDataInterface
{
    //LTC数据对外查询接口
     [RoutePrefix("api/Ext/Res")]
    public class ExtResidentController : BaseController
    {
        private readonly IResidentManageService _residentManageService = IOCContainer.Instance.Resolve<IResidentManageService>();
        [Route("GetResByIdNos"), HttpPost]
        public IHttpActionResult GetResidentsByIdNoList(List<string> resIdNos)
        {
            var response = _residentManageService.GetResidentsForExtApiByIdNoList(resIdNos);
            return Ok(response);
        }
        [Route("GetResByNsNo"), HttpPost]
        public IHttpActionResult GetResByNsNo(string nsNo)
        {
            //var response = _residentManageService.GetResidentsByIdNoList(resIdNos);
            //return Ok(response);
            return null;
        }
    }
}

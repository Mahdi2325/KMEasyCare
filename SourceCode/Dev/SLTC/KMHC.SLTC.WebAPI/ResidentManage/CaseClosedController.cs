using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KM.Common;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.WebAPI.ResidentManage
{
    [RoutePrefix("api/caseClosed")]
    public class CaseClosedController : BaseController
    {
        readonly IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();


        // GET api/syteminfo/5
        [Route("{FeeNo}")]
        public IHttpActionResult Get(long FeeNo)
        {
            var response = service.GetCloseCase(FeeNo);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(CloseCase baseRequest)
        {
            var response = service.SaveCloseCase(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{FeeNo}")]
        public IHttpActionResult Delete(long FeeNo)
        {
            var response = service.DeleteCloseCase(FeeNo);
            return Ok(response);
        }
    }
}


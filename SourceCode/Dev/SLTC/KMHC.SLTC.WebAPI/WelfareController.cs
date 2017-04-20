using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KM.Common;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System.Threading.Tasks;
using KMHC.Infrastructure;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/welfare")]
    public class WelfareController : BaseController
    {
         readonly IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();
         IResidentManageService regservice = IOCContainer.Instance.Resolve<IResidentManageService>();

        // GET api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Get(long id)
        {
            var response = service.GetResidentDtl(id);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(ResidentDtl baseRequest)
        {
            var response = service.SaveResidentDtl(baseRequest);
            return Ok(response);
        }


        [Route(""), HttpGet]
        public async Task<object> Query(string idNo)
        {
            var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/Appcert?idNo=" + idNo + "&type=" + SecurityHelper.CurrentPrincipal.OrgId);
            object resultContent = await result.Content.ReadAsAsync<object>();
            return resultContent;
        }


        // DELETE api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteResidentDtl(id);
            return Ok(response);
        }
    }
}

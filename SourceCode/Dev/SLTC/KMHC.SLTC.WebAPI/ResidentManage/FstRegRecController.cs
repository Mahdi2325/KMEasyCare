using KM.Common;
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

namespace KMHC.SLTC.WebAPI.ResidentManage
{
    [RoutePrefix("api/FstRegRec")]
    public class FstRegRecController :BaseController
    {
        IFstRegRecService service = IOCContainer.Instance.Resolve<IFstRegRecService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(int regNo)
        {
            var response = service.GetFstRegRec(regNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(FstRegRec baseRequest)
        {
            var response = service.SaveFstRegRec(baseRequest);
            return Ok(response);
        }
    }
}

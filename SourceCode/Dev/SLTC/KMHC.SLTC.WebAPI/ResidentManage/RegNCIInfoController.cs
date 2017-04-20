using KM.Common;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
   [RoutePrefix("api/regNCIInfo")]
   public class RegNCIInfoController : BaseController
    {
       IRegNCIInfoService service = IOCContainer.Instance.Resolve<IRegNCIInfoService>();
       [Route("")]
       public IHttpActionResult Post(RegNCIInfo baseRequest)
       {
           var response = service.SaveRegNCIInfo(baseRequest);
           return Ok(response);
       }

       [Route(""),HttpGet]
       public IHttpActionResult Get(int feeNo)
       {
           var response = service.GetLTCRegInfo(feeNo);
           return Ok(response);
       }

      [Route("welfareInfo") , HttpPost]
       public IHttpActionResult PostCareIns(CareInsInfo baseRequest)
       {
           var response = service.UpdateRegInfo(baseRequest);
           return Ok(response);
       }
    }
}

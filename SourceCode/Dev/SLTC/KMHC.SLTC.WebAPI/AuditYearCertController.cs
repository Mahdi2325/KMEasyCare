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
      [RoutePrefix("api/AuditYearCert")]
    public class AuditYearCertController : BaseController
    {
          readonly IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();
          [Route("")]
          public IHttpActionResult Post(AuditYearCertModel baseRequest)
          {
              var response = service.UpdateYearCert(baseRequest);
              return Ok(response);
          }
    }
    
}

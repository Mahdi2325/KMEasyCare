using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.NursingWorkstation;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NursingWorkstation
{
     [RoutePrefix("api/HandoverRecord")]
    public class HandoverRecordController : BaseController
    {
         IHandoverService service = IOCContainer.Instance.Resolve<IHandoverService>();
         [Route("{date}")]
         public IHttpActionResult Get(DateTime date)
         {
             var response = service.GetHandoverRecordByDate(date);
             return Ok(response);
         }

         [Route("saveHandoverDtl"), HttpPost]
         public IHttpActionResult PostHandoverDtl(LTC_HandoverDtl handoverDtl)
         {
             var response = service.SaveHandoverDtl(handoverDtl);
             return Ok(response);
         }
    }
}

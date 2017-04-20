using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.CrossSpeciality
{
   [RoutePrefix("api/DCProfessionalteamHis")]

    public class DC_MultiTeamHisController : BaseController
    {
       IDC_TransdisciplinaryPlan service = IOCContainer.Instance.Resolve<IDC_TransdisciplinaryPlan>();
         [Route("")]
       public IHttpActionResult Get(long feeNo)
       {

           var response = service.QueryTransdisciplinaryHistory(feeNo);
           return Ok(response);
          
       }

         [Route("")]
         public IHttpActionResult Get(long seqNo, long feeNo)
         {
             var response = service.QueryMultiCarePlanRec(seqNo);
             return Ok(response);
         }


        // DELETE api/syteminfo/5
         [Route("")]
         public IHttpActionResult Delete(long seqNo)
         {
             var response = service.DeleteTransdisciplinaryHis(seqNo);
             return Ok(response);
         }

    }
}

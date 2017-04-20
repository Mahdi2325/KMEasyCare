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
   [RoutePrefix("api/DCProfessionalteamList")]
    public class DC_MultiTeamController : BaseController
    {
       IDC_TransdisciplinaryPlan service = IOCContainer.Instance.Resolve<IDC_TransdisciplinaryPlan>();

       [Route("")]
       public IHttpActionResult Get([FromUri]long feeNo)
       {
           var response = service.QueryLatestMultiCarePlanRec(feeNo);

           return Ok(response);
       }

       [Route("")]
       public IHttpActionResult Post(DC_MultiteamCarePlanRecModel baseRequest)
       {
           var response = service.SaveTransdisciplinary(baseRequest);
           return Ok(response);
       }


    }
}

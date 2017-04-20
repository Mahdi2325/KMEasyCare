using KM.Common;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.CrossSpeciality
{
   [RoutePrefix("api/DCNurseCareLifeEdit")]
  public  class NurseingLifeEditController:BaseController
   {

       IDC_CrossDayLife service = IOCContainer.Instance.Resolve<IDC_CrossDayLife>();
       //[Route("")]
       //public IHttpActionResult Get([FromUri]string id)
       //{
       //    var response = service.QueryShowNurseList(id);

       //    return Ok(response);
       //}

       [Route("")]
       public IHttpActionResult Get([FromUri]string code)
       {
      
           var response = service.GetTeamAct(code);

           return Ok(response);
       }

       //保存的
       [Route("")]
       public IHttpActionResult Get()
       {
           //baseRequest.DayLifeRec.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
           var response = service.getCY();

           return Ok(response);
       }


   }
}

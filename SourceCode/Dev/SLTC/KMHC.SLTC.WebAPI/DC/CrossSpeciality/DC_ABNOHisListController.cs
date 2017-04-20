using KM.Common;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;


namespace KMHC.SLTC.WebAPI.DC.CrossSpeciality
{
  [RoutePrefix("api/DCProblemBehaviorHisList")]
    public class DC_ABNOHisListController : BaseController
    {
      IDC_CrossDayLife service = IOCContainer.Instance.Resolve<IDC_CrossDayLife>();
    
      [Route("")]
      public IHttpActionResult Get([FromUri]int FeeNo)
      {
          var response = service.QueryHISAB(FeeNo);

          return Ok(response);

      }

    }
}

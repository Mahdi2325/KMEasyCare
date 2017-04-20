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
    [RoutePrefix("api/DCDayLifeCarelistA")]
    public class DC_CrossLcontroller : BaseController
    {
        IDC_CrossDayLife service = IOCContainer.Instance.Resolve<IDC_CrossDayLife>();
        [Route("")]
        public IHttpActionResult Get([FromUri]string id)
        {
            var response = service.QueryShowDayLifeList(id);

            return Ok(response);
        }
    }
}

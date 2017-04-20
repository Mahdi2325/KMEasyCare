using KM.Common;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NursingWorkstation
{
    [RoutePrefix("api/workItem")]
    public class WorkItemController : BaseController
    {
        IHandoverService service = IOCContainer.Instance.Resolve<IHandoverService>();
        [Route("{itemType}")]
        public IHttpActionResult Get(string itemType)
        {
            var response = service.GetWorkItemByType(itemType);
            return Ok(response);
        }
    }
}

using KM.Common;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.ReceptionManagement
{
     [RoutePrefix("api/FrontConsole")]
    public class FrontConsoleController:BaseController
    {
         IReceptionManagementService _service = IOCContainer.Instance.Resolve<IReceptionManagementService>();
         [Route(""), HttpGet]
         public IHttpActionResult GetMonthData(DateTime beginTime, DateTime endTime)
         {
             var response = _service.GetFrontConsole(beginTime, endTime);
             return Ok(response);
         }
    }
}

using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.ReceptionManagement
{
    [RoutePrefix("api/advisoryRegCallBack")]
    public class AdvisoryRegCallBackController:BaseController
    {
        IReceptionManagementService _service = IOCContainer.Instance.Resolve<IReceptionManagementService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, DateTime? startTime, DateTime? endTime, int consultRecId)
        {
            BaseRequest<ConsultCallBackFilter> request = new BaseRequest<ConsultCallBackFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { CallBackStartTime = startTime, CallBackEndTime = endTime, ConsultRecId=consultRecId }
            };
            var response = _service.QueryConsultCallBack(request);
            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(ConsultCallBack baseRequest)
        {
            var response = _service.SaveConsultCallBack(baseRequest);
            return Ok(response);
        }
        [Route("{Id}")]
        public IHttpActionResult Delete(int Id)
        {
            var response = _service.DeleteConsultCallBack(Id);
            return Ok(response);
        }
        [Route("{Id}")]
        public IHttpActionResult Get(int Id)
        {
            var response = _service.GetConsultCallBack(Id);
            return Ok(response);
        }
    }
}

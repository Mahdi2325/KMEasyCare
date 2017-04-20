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
    [RoutePrefix("api/advisoryReg")]
    public class AdvisoryRegController : BaseController
    {
        IReceptionManagementService _service = IOCContainer.Instance.Resolve<IReceptionManagementService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, string keyWords,DateTime? sDate,DateTime? eDate)
        {
            BaseRequest<AdvisoryRegFilter> request = new BaseRequest<AdvisoryRegFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { KeyWords = keyWords, ConsultStartTime=sDate,ConsultEndTime=eDate }
            };
            var response = _service.QueryAdvisoryReg(request);
            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(ConsultRec baseRequest)
        {
            var response = _service.SaveConsultRec(baseRequest);
            return Ok(response);
        }
        [Route("{Id}")]
        public IHttpActionResult Delete(int Id)
        {
            var response = _service.DeleteConsultRec(Id);
            return Ok(response);
        }
        [Route("{Id}")]
        public IHttpActionResult Get(int Id)
        {
            var response = _service.GetConsultRec(Id);
            return Ok(response);
        }
    }
}

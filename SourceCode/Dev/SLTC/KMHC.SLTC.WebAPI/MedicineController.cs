using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;
using KM.Common;

using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Base;
using System;
using KMHC.SLTC.Business.Entity;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/medicines")]
    public class MedicineController:BaseController
    {

        INursingManageService service = IOCContainer.Instance.Resolve<INursingManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult get()
        {
            BaseRequest<MedicineFilter> request = new BaseRequest<MedicineFilter>();
            var response = service.QueryMedData(request);
            return Ok(response.Data);
        }

        [Route("")]
        public IHttpActionResult post(Medicine request)
        {
            try
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            catch
            {
                request.OrgId = "1";
            }
            var response = service.SaveMedData(request);
            return Ok(response.Data);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteMedData(id);
            return Ok(response);
        }
    }
}

using KM.Common;
using KMHC.Infrastructure;
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

namespace KMHC.SLTC.WebAPI.ChargeItem
{
    [RoutePrefix("api/nsmaterialmgr")]
    public class NSMedicalMaterialManageController : BaseController
    {

        INSMedicalMaterialManageService service = IOCContainer.Instance.Resolve<INSMedicalMaterialManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(string keyWord="", int currentPage = 1, int pageSize = 100)
        {
            BaseRequest<NSMedicalMaterialFilter> request = new BaseRequest<NSMedicalMaterialFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { KeyWord = keyWord}
            };
            var response = service.Query(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(int status,string keyWord = "", int currentPage = 1, int pageSize = 100)
        {
            BaseRequest<NSMedicalMaterialFilter> request = new BaseRequest<NSMedicalMaterialFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { KeyWord = keyWord ,Status= status }
            };
            var response = service.Query(request);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = service.Get(id);
            return Ok(response);
        }

        [Route("getbymcmaterialcode")]
        public IHttpActionResult GetByMCMedicalMaterialCode(string mcMaterialCode)
        {
            var response = service.GetByMCMedicalMaterialCode(mcMaterialCode);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(NSMedicalMaterial baseRequest)
        {
            var response = service.Save(baseRequest);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.Delete(id);
            return Ok(response);
        }
    }
}

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
    [RoutePrefix("api/nsdrugmgr")]
    public class NSDrugManageController : BaseController
    {

        INSDrugManageService service = IOCContainer.Instance.Resolve<INSDrugManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(string keyWord="", int currentPage = 1, int pageSize = 100)
        {
            BaseRequest<NSDrugFilter> request = new BaseRequest<NSDrugFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { KeyWord = keyWord }
            };
            var response = service.Query(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult QueryUsableItem(int status, string keyWord = "" ,int currentPage = 1, int pageSize = 100)
        {
            BaseRequest<NSDrugFilter> request = new BaseRequest<NSDrugFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { KeyWord = keyWord, Status = status }
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

        [Route("getbymcdrugcode")]
        public IHttpActionResult GetByMCDrugCode(string mcDrugCode)
        {
            var response = service.GetByMCDrugCode(mcDrugCode);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(NSDrug baseRequest)
        {
            baseRequest.NSId = SecurityHelper.CurrentPrincipal.OrgId;
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

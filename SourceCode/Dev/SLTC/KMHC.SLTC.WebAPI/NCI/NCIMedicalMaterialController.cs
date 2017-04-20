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
    [RoutePrefix("api/ncimaterial")]
    public class NCIMedicalMaterialController : BaseController
    {

        INCIMedicalMaterialService service = IOCContainer.Instance.Resolve<INCIMedicalMaterialService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query([FromUri]string keyWord, int currentPage = 1, int pageSize = 100)
        {
            if (string.IsNullOrWhiteSpace(keyWord))
                keyWord = "";

            BaseRequest<NCIMedicalMaterialFilter> request = new BaseRequest<NCIMedicalMaterialFilter> 
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = {KeyWord = keyWord}
            };
            var response = service.Query(request);
            return Ok(response);
        }

        [Route("{materialcode}")]
        public IHttpActionResult Get(string materialCode)
        {
            var response = service.Get(materialCode);
            return Ok(response);
        }
    }
}

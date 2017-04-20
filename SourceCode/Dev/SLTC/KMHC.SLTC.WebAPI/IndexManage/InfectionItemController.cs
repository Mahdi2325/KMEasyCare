using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.IndexManage
{
    [RoutePrefix("api/InfectionItem")]
    public class InfectionItemController : BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage = 1, int pageSize = 50)
        {
            BaseRequest<InfectionItemFilter> request = new BaseRequest<InfectionItemFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            var response = service.QueryInfectionItem(request);
            return Ok(response);
        }
    }
}

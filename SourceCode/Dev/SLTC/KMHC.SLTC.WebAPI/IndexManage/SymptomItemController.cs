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
    [RoutePrefix("api/SymptomItem")]
    public class SymptomItemController : BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(string ItemCode, int currentPage = 1, int pageSize = 100)
        {
            BaseRequest<SymptomItemFilter> request = new BaseRequest<SymptomItemFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.ItemCode = ItemCode;
            var response = service.QuerySymptomItem(request);
            return Ok(response);
        }
    }
}

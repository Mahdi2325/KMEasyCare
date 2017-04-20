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

namespace KMHC.SLTC.WebAPI.SocialWorker
{
     [RoutePrefix("api/RegActivityRequEval")]
   public class RegActivityRequEvalController:BaseController
    {
       ISocialWorkerManageService service = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        [Route(""), HttpGet]
       public IHttpActionResult get(int feeNo, int CurrentPage, int PageSize)
        {
            RegActivityRequEvalFilter filter = new RegActivityRequEvalFilter
            {
                FeeNo = feeNo
            };
            BaseRequest<RegActivityRequEvalFilter> request = new BaseRequest<RegActivityRequEvalFilter>
            {
                Data = filter
            };
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            var response = service.QueryActivityRequEval(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(RegActivityRequEval request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveActivityRequEval(request);
            return Ok(response.Data);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteActivityRequEval(id);
            return Ok(response);
        }
    }
}

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

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/pains")]
    public class PainEvalController : BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(long feeNo = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<PainEvalRecFilter> request = new BaseRequest<PainEvalRecFilter>()
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = new PainEvalRecFilter()
                {
                    FeeNo = feeNo,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId
                }
            };
            var response = service.QueryPainEvalRecExtend(request);
            return Ok(response);
        }

        // GET
        [Route("{FeeNo}"), HttpGet]
        public IHttpActionResult Get(long feeNo)
        {
            BaseRequest<PainEvalRecFilter> request = new BaseRequest<PainEvalRecFilter>()
            {
                Data = new PainEvalRecFilter()
                {
                    FeeNo = feeNo,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId
                }
            };
            var response = service.GetPainEvalRecExtend(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(PainEvalRec request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            request.CreateBy = SecurityHelper.CurrentPrincipal.LoginName;
            request.CreateDate = DateTime.Now;
            var response = service.SavePainEvalRec(request);
            return Ok(response);
        }


        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeletePainEvalRec(id);
            return Ok(response);
        }
    }
}

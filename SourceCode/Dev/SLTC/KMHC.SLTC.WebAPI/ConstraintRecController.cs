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
    [RoutePrefix("api/restricts")]
    public class ConstraintRecController : BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(long feeNo = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<ConstraintRecFilter> request = new BaseRequest<ConstraintRecFilter>()
            {
                Data = new ConstraintRecFilter()
                {
                    FeeNo = feeNo,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId
                },
                CurrentPage = currentPage,
                PageSize = pageSize
            };
            var response = service.QueryConstraintRecExtend(request);
            return Ok(response);
        }

        // GET
        [Route("{FeeNo}"), HttpGet]
        public IHttpActionResult Get(long feeNo)
        {
            BaseRequest<ConstraintRecFilter> request = new BaseRequest<ConstraintRecFilter>()
            {
                Data = new ConstraintRecFilter()
                {
                    FeeNo = feeNo,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId
                }
            };
            var response = service.GetConstraintRecExtend(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(ConstraintRec request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            request.CreateBy = SecurityHelper.CurrentPrincipal.LoginName;
            request.CreateDate = DateTime.Now;
            var response = service.SaveConstraintRec(request);
            return Ok(response);
        }


        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteConstraintRec(id);
            return Ok(response);
        }
    }
}

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
    [RoutePrefix("api/complainrecs")]
    public class ComplainRecController:BaseController
    {
        ISocialWorkerManageService service = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, int CurrentPage, int PageSize)
        {
            BaseRequest<ComplainRecFilter> request = new BaseRequest<ComplainRecFilter>();

            request.CurrentPage = CurrentPage;

            request.PageSize = PageSize;

            request.Data.FeeNo = feeNo;

            var response = service.QueryComplainRec(request);

            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(ComplainRecModel baseRequest)
        {
            baseRequest.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            baseRequest.CreateDate = DateTime.Now;
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;

            var response = service.SaveComplainRec(baseRequest);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetComplainRecById(id);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteComplainRecById(id);
            return Ok(response);
        }
    }
}

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
    [RoutePrefix("api/referralrecs")]
    public class ReferralRecController:BaseController
    {
        ISocialWorkerManageService service = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        //GET api/SubsidyRec
        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, int CurrentPage, int PageSize)
        {
            BaseRequest<ReferralRecFilter> request = new BaseRequest<ReferralRecFilter>();

            request.CurrentPage = CurrentPage;

            request.PageSize = PageSize;

            request.Data.FeeNo = feeNo;

            var response = service.QueryReferralRec(request);

            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(ReferralRecModel baseRequest)
        {
            baseRequest.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            baseRequest.CreateDate = DateTime.Now;
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;


            var response = service.SaveReferral(baseRequest);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetReferralById(id);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteReferralById(id);
            return Ok(response);
        }
    }
}

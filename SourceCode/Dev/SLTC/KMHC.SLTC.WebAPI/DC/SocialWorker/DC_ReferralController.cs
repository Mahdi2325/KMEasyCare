using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;

namespace KMHC.SLTC.WebAPI.DC.SocialWorker
{
    [RoutePrefix("api/PersonReferral")]
    public class DC_ReferralController:BaseController
    {
        IDC_SocialWorkerService service = IOCContainer.Instance.Resolve<IDC_SocialWorkerService>();

        //GET api/Referral
        [Route(""), HttpGet]
        public IHttpActionResult Query(string feeNo, int currentPage, int pageSize)
        {
            BaseRequest<DC_ReferrallistsFilter> request = new BaseRequest<DC_ReferrallistsFilter>();

            request.CurrentPage = currentPage;

            request.PageSize = pageSize;

            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;

            request.Data.FeeNo = Convert.ToInt32(feeNo);

            var response = service.QueryReferrallist(request);

            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(DC_ReferrallistsModel baseRequest)
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

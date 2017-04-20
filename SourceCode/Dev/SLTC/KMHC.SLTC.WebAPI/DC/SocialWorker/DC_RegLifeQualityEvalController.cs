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
    [RoutePrefix("api/RegLifeQualityEval")]
    public class DC_RegLifeQualityEvalController:BaseController
    {
        IDC_SocialWorkerService service = IOCContainer.Instance.Resolve<IDC_SocialWorkerService>();

        //GET api/Referral
        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, int currentPage, int pageSize)
        {
            BaseRequest<DC_RegLifeQualityEvalFilter> request = new BaseRequest<DC_RegLifeQualityEvalFilter>();

            request.CurrentPage = currentPage;

            request.PageSize = pageSize;

            request.Data._orgid = SecurityHelper.CurrentPrincipal.OrgId;

            request.Data._feeno = Convert.ToInt32(feeNo);

            var response = service.QueryRegLifeQualityEval(request);

            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(DC_RegLifeQualityEvalModel baseRequest)
        {
            baseRequest.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            baseRequest.CreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;

            var response = service.SaveRegLifeQualityEval(baseRequest);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetRegLifeQualityEvalById(id);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteRegLifeQualityEvalById(id);
            return Ok(response);
        }
    }
}

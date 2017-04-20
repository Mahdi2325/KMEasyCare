using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;
using KMHC.Infrastructure;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/Infection")]
    public class InfectionController : BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(long regNo = 0, long feeNo = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<InfectionIndFilter> request = new BaseRequest<InfectionIndFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.RegNo = regNo;
            request.Data.FeeNo = feeNo;
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.QueryInfectionInd(request);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Get(int feeNo)
        {
            var response = service.GetInfectionInd(feeNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(InfectionInd baseRequest)
        {
            var response = service.SaveInfectionInd(baseRequest);
            return Ok(response);
        }

        [Route("{seqNo}")]
        public IHttpActionResult Delete(long seqNo)
        {
            var response = service.DeleteInfectionInd(seqNo);
            return Ok(response);
        }
    }
}
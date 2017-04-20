using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/RelationDtl")]
    public class RelationDtlController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, int currentPage, int pageSize)
        {
            BaseRequest<RelationDtlFilter> request = new BaseRequest<RelationDtlFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.FeeNo = feeNo;
            var response = service.QueryRelationDtl(request);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Get(int feeNo)
        {
            var response = service.GetRelationDtl(feeNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(RelationDtl baseRequest)
        {
            var response = service.SaveRelationDtl(baseRequest);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Delete(int feeNo)
        {
            var response = service.DeleteRelationDtl(feeNo);
            return Ok(response);
        }
    }
}
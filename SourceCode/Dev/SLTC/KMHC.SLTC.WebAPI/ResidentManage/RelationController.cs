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
    [RoutePrefix("api/Relation")]
    public class RelationController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize)
        {
            BaseRequest<RelationFilter> request = new BaseRequest<RelationFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            var response = service.QueryRelation(request);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Get(int feeNo)
        {
            var response = service.GetRelation(feeNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(Relation baseRequest)
        {
            var response = service.SaveRelation(baseRequest);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Delete(int feeNo)
        {
            var response = service.DeleteRelation(feeNo);
            return Ok(response);
        }
    }
}
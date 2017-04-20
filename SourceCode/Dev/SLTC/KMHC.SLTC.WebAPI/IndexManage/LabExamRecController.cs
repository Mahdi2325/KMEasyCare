namespace KMHC.SLTC.WebAPI
{
    using KM.Common;
    using KMHC.SLTC.Business.Entity;
    using KMHC.SLTC.Business.Entity.Base;
    using KMHC.SLTC.Business.Entity.Filter;
    using KMHC.SLTC.Business.Entity.Model;
    using KMHC.SLTC.Business.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("api/LabExamRec")]
    public class LabExamRecController : BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(long seqNo = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<LabExamRecFilter> request = new BaseRequest<LabExamRecFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.SeqNo = seqNo;
            var response = service.QueryLabExamRec(request);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Get(long id)
        {
            var response = service.GetLabExamRec(id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(LabExamRec baseRequest)
        {
            var response = service.SaveLabExamRec(baseRequest);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteLabExamRec(id);
            return Ok(response);
        }
    }
}

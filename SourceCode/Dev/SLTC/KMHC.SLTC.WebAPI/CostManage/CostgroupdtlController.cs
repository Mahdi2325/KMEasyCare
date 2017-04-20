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
    
    [RoutePrefix("api/CostGroupDtl")]
    public class CostGroupDtlController : BaseController
    {
        ICostManageService service = IOCContainer.Instance.Resolve<ICostManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(BaseRequest<CostGroupDtlFilter> request)
        {
            var response = service.QueryCostGroupDtl(request);
            return Ok(response);
        }


        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            var response = new BaseResponse();
            if (!string.IsNullOrEmpty(id)) {
                var idAry = id.Split(',').ToList();
                response = service.DeleteCostGroupDtl(idAry);
            }
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(CostGroupDtl baseRequest)
        {
            var response = service.SaveCostGroupDtl(baseRequest);
            return Ok(response);
        }
    }
}

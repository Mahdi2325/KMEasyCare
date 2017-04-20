namespace KMHC.SLTC.WebAPI
{
    using KM.Common;
    using KMHC.Infrastructure;
    using KMHC.SLTC.Business.Entity;
    using KMHC.SLTC.Business.Entity.Base;
    using KMHC.SLTC.Business.Entity.Filter;
    using KMHC.SLTC.Business.Entity.Model;
    using KMHC.SLTC.Business.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("api/CostGroup")]
    public class CostgroupController : BaseController
    {
        ICostManageService service = IOCContainer.Instance.Resolve<ICostManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage = 1, int pageSize = 100, string groupName = "")
        {
            var request = new BaseRequest<CostGroupFilter>()
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = new CostGroupFilter()
                {
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                    GroupName = groupName
                }
            };


            var response = service.QueryCostGroup(request);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetCostGroupExtend(id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(CostGroup baseRequest)
        {
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveCostGroup(baseRequest);
            if (response.ResultCode != 1001)
            {
                response.Data.GroupItems = null;
            }
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = new BaseResponse();
            service.DeleteCostGroupDtlByGroupId(id);
            response = service.DeleteCostGroup(id);
            return Ok(response);
        }
    }
}

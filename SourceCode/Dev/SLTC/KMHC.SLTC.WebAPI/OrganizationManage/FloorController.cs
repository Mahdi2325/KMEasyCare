using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
using KM.Common;
using KMHC.Infrastructure;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/floors")]
    public class FloorController : BaseController
    {
        IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        // GET 
        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage = 1, int pageSize = 10, string floorName = "")
        {
            BaseRequest<OrgFloorFilter> request = new BaseRequest<OrgFloorFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FloorName = floorName, OrgId = SecurityHelper.CurrentPrincipal.OrgId }
            };
            var response = organizationManageService.QueryOrgFloorExtend(request);
            return Ok(response);
        }

        // GET api/syteminfo/5
        [Route("{FloorId}")]
        public IHttpActionResult Get(string FloorId)
        {
            var response = organizationManageService.GetOrgFloor(FloorId);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post([FromBody]OrgFloor baseRequest)
        {
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            //baseRequest.OrgName = SecurityHelper.CurrentPrincipal.;
            var response = organizationManageService.SaveOrgFloor(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{FloorId}")]
        public IHttpActionResult Delete(string FloorId)
        {
            var response = organizationManageService.DeleteOrgFloor(FloorId);
            return Ok(response);
        }
    }
}

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
    [RoutePrefix("api/organizations")]
    public class OrgController : BaseController
    {
        IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();


        [Route(""), HttpGet]
        public IHttpActionResult Query(string OrgName = "",int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<OrganizationFilter> request = new BaseRequest<OrganizationFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { OrgName = OrgName }
            };
            var response = organizationManageService.QueryOrg(request);
            return Ok(response);
        }

        // GET api/syteminfo/5
        [Route("{orgID}")]
        public IHttpActionResult Get(string orgID)
        {
            var response = organizationManageService.GetOrg(orgID);
            return Ok(response);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(Organization baseRequest)
        {
            //if (string.IsNullOrEmpty(baseRequest.OrgId))
            //{
            //    baseRequest.OrgId = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            //}
            var response = organizationManageService.SaveOrg(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{orgID}")]
        public IHttpActionResult Delete(string orgID)
        {
            var response = organizationManageService.DeleteOrg(orgID);
            return Ok(response);
        }
    }
}
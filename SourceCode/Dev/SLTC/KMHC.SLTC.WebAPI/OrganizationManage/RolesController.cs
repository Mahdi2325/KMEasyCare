using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;


namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/roles")]
    public class RolesController : BaseController
    {
        IOrganizationManageService RolesService = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        // GET api/Floor
        [Route(""), HttpGet]
        public IHttpActionResult Query(string roleName = "", string roleType = "", string orgid="",bool? status = null, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<RoleFilter> request = new BaseRequest<RoleFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { RoleName = roleName, OrgId = SecurityHelper.CurrentPrincipal.OrgId, RoleType = roleType, Status = status, CurrentLoginSys = SecurityHelper .CurrentPrincipal.CurrentLoginSys}
            };

            //if (roleType == EnumRoleType.SuperAdmin.ToString())
            //{
            //    request.Data.OrgId = string.Empty;
            //}
            //else
            //{
                //自定义机构 针对超级管理员zhongyh
                if (!string.IsNullOrEmpty(orgid))
                { request.Data.OrgId = orgid; }
            //}
            var response = RolesService.QueryRole(request);
            return Ok(response);
        }


        // GET api/syteminfo/5
        [Route("{RoleId}")]
        public IHttpActionResult Get(string RoleId)
        {
            var response = RolesService.GetRole(RoleId);
            return Ok(response);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(Role baseRequest)
        {
            if (string.IsNullOrEmpty(baseRequest.RoleId))
            {
                baseRequest.RoleId = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            }
            if (string.IsNullOrEmpty(baseRequest.OrgId))//如果为空默认当前机构zhongyh
            {
                baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            
            var response = RolesService.SaveRole(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{RoleId}")]
        public IHttpActionResult Delete(string RoleId)
        {
            var response = RolesService.DeleteRole(RoleId);
            return Ok(response);
        }
    }
}
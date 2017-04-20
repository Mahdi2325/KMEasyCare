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
using KMHC.Infrastructure;


namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/module")]
    public class ModuleController : BaseController
    {
        readonly IOrganizationManageService _service = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query()
        {
            //SecurityHelper.CurrentPrincipal.RoleId;
            RoleFilter request = new RoleFilter();
            request.RoleId = SecurityHelper.CurrentPrincipal.RoleId;
            var response = _service.GetRoleModule(request).ToList().Distinct();
            //var response = _service.GetMenus(roleId);
            return Ok(response);
        }

        [Route("{roleId}")]
        public IHttpActionResult Get(string roleId, string type = "", bool loadTreeByRole = false)
        {

            if (type == "")
            {
                RoleFilter request = new RoleFilter
                {
                    RoleId = string.IsNullOrWhiteSpace(roleId) ? null : roleId.Split(',')
                };
                var response = _service.GetRoleModule(request);
                return Ok(response);
            }
            else if (type == "tree")
            {
                BaseRequest<RoleFilter> requestByRole = new BaseRequest<RoleFilter>
                {
                    Data = {RoleId = string.IsNullOrWhiteSpace(roleId) ? null : roleId.Split(',')}
                };

                BaseRequest<RoleFilter> requestByTree = new BaseRequest<RoleFilter>();
                if (loadTreeByRole)
                {
                    requestByTree.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                    //requestByTree.Data.RoleType = EnumRoleType.SuperAdmin.ToString();
                    if (SecurityHelper.CurrentPrincipal.CurrentLoginSys=="LC")
                    requestByTree.Data.RoleType = SecurityHelper.CurrentPrincipal.LTCRoleType[0];
                    else
                   requestByTree.Data.RoleType = SecurityHelper.CurrentPrincipal.DCRoleType[0];
                }

                var response = _service.GetModuleByRole(requestByRole, requestByTree);
                return Ok(response);
            }
            else
            {
                return Ok("type参数不正确");
            }
        }

        //// GET api/syteminfo/5
        //[Route("roleId")]
        //public IHttpActionResult Get(string roleId)
        //{
        //    var list = _service.GetRoleModule(roleId).ToList();
        //    var response = CreateTree(list, "00");
        //    return Ok(response);
        //}



    }


}

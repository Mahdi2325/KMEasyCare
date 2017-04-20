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
    [RoutePrefix("api/Depts")]
    public class DeptController : BaseController
    {
        IOrganizationManageService DeptService = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage = 1, int pageSize = 100, string name = "", string id = "", string orgid = "")
        {
            if (name == null || name == "undefined")
            { name = ""; }
            BaseRequest<DeptFilter> request = new BaseRequest<DeptFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { 
                    DeptName = name,
                    DeptNo = name,
                    OrgId=SecurityHelper.CurrentPrincipal.OrgId
                }
            };
            //自定义机构查询 针对超级管理员zhongyh
            if (!string.IsNullOrEmpty(orgid))
            { request.Data.OrgId = orgid; }

             
            var response = DeptService.QueryDeptExtend(request);
            return Ok(response);
        }


        // GET api/syteminfo/5
        [Route("{DeptNo}")]
        public IHttpActionResult Get(string DeptNo)
        {
            var response = DeptService.GetDept(DeptNo);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(Dept baseRequest)
        {
            if (string.IsNullOrEmpty(baseRequest.DeptNo))
            {
                baseRequest.DeptNo = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            }
            baseRequest.UpdateBy = SecurityHelper.CurrentPrincipal.LoginName;
            baseRequest.UpdateDate = DateTime.Now;
            var response = DeptService.SaveDept(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("")]
        public IHttpActionResult Delete(string deptNo,string orgId)
        {
            var response = DeptService.DeleteDept(deptNo, orgId);
            return Ok(response);
        }
    }

}

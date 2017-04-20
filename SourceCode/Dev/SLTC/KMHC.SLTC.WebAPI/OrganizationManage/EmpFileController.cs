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

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/empfiles")]
    public class EmpFileController : BaseController
    {
        IOrganizationManageService empFileService = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        // GET api/Floor
        [Route(""), HttpGet]
        public IHttpActionResult Query(string empNo = "", string empName = "", string empGroup = "", string orgid = "", int currentPage = 1, int pageSize = 10, int needEmp = 0, int needShort = 0)
        {
            BaseRequest<EmployeeFilter> request = new BaseRequest<EmployeeFilter>()
            {
                CurrentPage = currentPage,
                PageSize = pageSize,

                Data =
                {
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                    EmpNo = empNo,
                    EmpName = empName,
                    EmpGroup = empGroup,
                    NeedEmp = needEmp
                }
            };

            //自定义机构 针对超级管理员zhongyh
            if (!string.IsNullOrEmpty(orgid))
            { request.Data.OrgId = orgid; }


            var orginfo = empFileService.GetOrg(request.Data.OrgId);//取机构名称

            var response = new BaseResponse<IList<Employee>>();
            if (needEmp > 0)
            {
                var responseObject = new BaseResponse<List<object>>();
                response = empFileService.QueryUserUnionEmp(request);
            }
            else
            {
                response = empFileService.QueryEmployee(request);
            }

            if (needShort > 0)
            {
                var responseObject = new BaseResponse<IList<Object>>()
                {
                    Data = new List<object>()
                };
                foreach (var item in response.Data)
                {
                    responseObject.Data.Add(new { EmpName = item.EmpName, EmpNo = item.EmpNo });
                }
                return Ok(responseObject);
            }
            else
            {

                foreach (var item in response.Data)
                {
                    item.OrgId = orginfo.Data.OrgName;
                }
                return Ok(response);
            }
        }

        // GET api/syteminfo/5
        [Route("{EmpNo}")]
        public IHttpActionResult Get(string EmpNo)
        {
            var response = empFileService.GetEmployee(EmpNo);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(Employee baseRequest)
        {
            //if (string.IsNullOrEmpty(baseRequest.EmpNo))
            //{
            //    baseRequest.EmpNo = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            //}
            if (string.IsNullOrEmpty(baseRequest.OrgId))
            {
                baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }

            var response = empFileService.SaveEmployee(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("")]
        public IHttpActionResult Delete(string empNo,string orgId)
        {
            var response = empFileService.DeleteEmployee(empNo, orgId);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, string name, string empGroup)
        {
            BaseRequest<EmployeeFilter> request = new BaseRequest<EmployeeFilter>()
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data =
                {
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                    EmpName = name,
                    EmpGroup = empGroup
                }
            };
            var response = empFileService.QueryEmployee(request);
            return Ok(response);
        }
    }
}

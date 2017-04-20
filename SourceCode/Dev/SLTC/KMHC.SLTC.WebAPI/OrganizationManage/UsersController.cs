using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;
using KMHC.Infrastructure;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/users")]
    public class UsersController : BaseController
    {
        IOrganizationManageService usersService = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        // GET api/Floor
        [Route(""), HttpGet]
        public IHttpActionResult Query(string keyWord, string orgid="",int currentPage=1, int pageSize=10,string fingtype="")
        {
            BaseRequest<UserFilter> request = new BaseRequest<UserFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { LoginName = keyWord, EmpNo = keyWord, Findtype = fingtype, OrgId = orgid, UserId = SecurityHelper.CurrentPrincipal.UserId, LoginSysType = SecurityHelper.CurrentPrincipal.CurrentLoginSys}
            };
            //有传入值就按传入的值来查，没有就按默认当前机构
            if (string.IsNullOrEmpty(orgid))
            {
                request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
       
            var response = usersService.QueryUserExtend(request);
            return Ok(response);
        }

        /// <summary>
        /// 校验用户登录名是否已经存在，校验相同员工是否已经绑定过登录账号 2016/9/1
        /// </summary>
        /// <param name="empNo"></param>
        /// <param name="loginName"></param>
        /// <param name="orgid"></param>
        /// <param name="fingtype"></param>
        /// <returns></returns>
        [Route(""), HttpGet]
        public IHttpActionResult Get(string empNo, string loginName, string orgid ,int userId, string fingtype)
        {
            BaseRequest<UserFilter> request = new BaseRequest<UserFilter>
            {
                Data = { LoginName = loginName, EmpNo = empNo, Findtype = fingtype, OrgId = orgid, UserId = userId }
            };
            //有传入值就按传入的值来查，没有就按默认当前机构
            if (string.IsNullOrEmpty(orgid))
            {
                request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            var response = usersService.CheckUserEmpAndLogName(request);
            return Ok(response);
        }

        // GET api/syteminfo/5
        [Route("{UserId}")]
        public IHttpActionResult Get(int UserId)
        {
            var response = usersService.GetUser(UserId);
            return Ok(response.Data);
        }


        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(User baseRequest)
        {
            if (baseRequest.UserId == 0)
            {
                baseRequest.Status = true;
                baseRequest.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                baseRequest.CreateDate = DateTime.Now;
            }
            else {
                baseRequest.UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                baseRequest.UpdateDate = DateTime.Now;
            }
            var response = usersService.SaveUser(baseRequest);
            return Ok(response);
        }
        [Route("ChangePassWord")]
        public IHttpActionResult ChangePassWord([FromBody] dynamic user)
        {
            var oldPass = user.oldPwd.Value;
            var newPass = user.newPwd.Value;
            string orgID = SecurityHelper.CurrentPrincipal.OrgId;
            string logonName = SecurityHelper.CurrentPrincipal.LoginName;
            var response = usersService.ChangePassWord(orgID,logonName,oldPass,newPass);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{UserId}")]
        public IHttpActionResult Delete(int UserId)
        {
            var response = usersService.DeleteUser(UserId);
            return Ok(response);
        }
    }
}
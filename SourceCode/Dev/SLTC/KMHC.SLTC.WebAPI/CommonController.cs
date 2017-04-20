using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Interface;
using KM.Common;
using KMHC.SLTC.Business.Entity.Filter;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/Common")]
    public partial class CommonController : BaseController
    {
        [Route(""), HttpGet]
        public IHttpActionResult Get()
        {
            var response = new BaseResponse<object>();
            response.Data = new { EmpNo = SecurityHelper.CurrentPrincipal.EmpNo };
            return Ok(response);
        }

        [Route("ValidateCode/SendEmail/{orgID}/{loginName}"), HttpGet]
        public IHttpActionResult SendEmail(string orgID, string loginName)
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            checkCode = checkCode.ToUpper();

            IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            BaseRequest<UserFilter> userFilter = new BaseRequest<UserFilter>();
            userFilter.Data.LoginName = loginName;
            var response = service.QueryUser(userFilter);
            ChangePassword request = new WebAPI.ChangePassword();
            request.OrgID = orgID;
            request.LoginName = loginName;
            if (response.Data.Count == 1)
            {
                request.Email = response.Data[0].Email;
            }
            request.ValidateCode = checkCode;
            SessionHelper.SetSession("UserInfo", request);

            var adminUserList = service.GetUsreByRoleType(request.OrgID, "Admin");
            List<string> toMailAddress = new List<string>();
            if (!string.IsNullOrEmpty(request.Email))
            {
                toMailAddress.Add(request.Email);
            }
            if (adminUserList.Data != null)
            {
                adminUserList.Data.ForEach(it =>
                {
                    if (!string.IsNullOrEmpty(it.Email))
                    {
                        toMailAddress.Add(it.Email);
                    }
                });
            }
            if (toMailAddress.Count > 0)
            {
                string senderServerIp = "smtp.163.com";
                //smtp.163.com
                //smtp.gmail.com
                //smtp.qq.com
                //smtp.sina.com;

                //DXiao@kmhealthcloud.com
                string fromMailAddress = "SLTC_Admin@163.com";
                string subjectInfo = "修改密码";
                string bodyInfo = string.Format("您好 {0}, 这是修改密码的验证码{1}。", loginName, checkCode);
                string mailUsername = "SLTC_Admin";
                string mailPassword = "kmadmin01"; //发送邮箱的密码（）
                string mailPort = "25";

                MyEmail email = new MyEmail(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);
                email.Send();
            }
            if (toMailAddress.Count > 0)
            {
                string msg = string.Empty;
                toMailAddress.ForEach(it=> {
                    int index = it.IndexOf("@") - 3;
                    if (index < 1)
                    {
                        index = 1;
                    }
                    msg = string.Format("{0}{1}*{2};", msg, it.Substring(0, 1), it.Substring(index, it.Length - index));
                });
                msg = string.Format("验证码已发送至：{0}", msg.TrimEnd(';'));
                return Ok(msg);
            }
            else
            {
                return Ok("您没有设置接收验收验收码的邮箱，请联系管理员。");
            }
        }

        [Route("ValidateLoginName"), HttpPost]
        public IHttpActionResult ValidateLoginName(ChangePassword request)
        {
            IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            BaseRequest<UserFilter> userFilter = new BaseRequest<UserFilter>();
            userFilter.Data.OrgId = request.OrgID;
            userFilter.Data.LoginName = request.LoginName;
            if( !string.IsNullOrEmpty(userFilter.Data.LoginName))
            {
                var response = service.QueryUser(userFilter);
                return Ok(response.Data.Count > 0);
            }
            else
            {
                return Ok(false);
            }
        }

        [Route("ValidateCode"), HttpPost]
        public IHttpActionResult ValidateCode(ChangePassword request)
        {
            string validateCode = ((ChangePassword)SessionHelper.Get("UserInfo")).ValidateCode;
            return Ok(request.ValidateCode.ToUpper() == validateCode);
        }

        [Route("ResetPassword"), HttpPost]
        public IHttpActionResult ChangePassword(ChangePassword request)
        {
            IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            string orgID = ((ChangePassword)SessionHelper.Get("UserInfo")).OrgID;
            string loginName = ((ChangePassword)SessionHelper.Get("UserInfo")).LoginName;
            var response = service.ResetPassword(orgID,loginName, request.NewPassword);
            return Ok(response.ResultCode == 0 ? "密码修改成功，请重回登录页登录。" : "密码修改失败，请稍後再试。");
        }

        [Route("{type}/{key}/{value}")]
        public IHttpActionResult Exists(string type, string key, string value, string p1 = "")
        {
            bool result = false;
            switch (type)
            {
                case "EmpIdNo":
                    {
                        BaseRequest<EmployeeFilter> request = new BaseRequest<EmployeeFilter>();
                        request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                        request.Data.IdNo = value;
                        IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
                        var response = service.QueryEmployee(request);
                        if (response.Data.Count > 0)
                        {
                            result = ((List<Employee>)response.Data).Any(it => it.EmpNo != key);
                        }
                        break;
                    }
                case "RegIdNo":
                    {
                        BaseRequest<PersonFilter> request = new BaseRequest<PersonFilter>();
                        request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                        request.Data.IdNo = value;
                        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();
                        var response = service.QueryPerson(request);
                        if (response.Data.Count > 0)
                        {
                            result = ((List<Person>)response.Data).Any(it => it.RegNo != int.Parse(key));
                        }
                        break;
                    }
                case "ResidengNo":
                    {
                        BaseRequest<PersonFilter> request = new BaseRequest<PersonFilter>();
                        request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                        request.Data.ResidengNo = value;
                        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();
                        var response = service.QueryPerson(request);
                        if (response.Data.Count > 0)
                        {
                            result = ((List<Person>)response.Data).Any(it => it.RegNo != int.Parse(key));
                        }
                        break;
                    }
               case "FloorId":
                    {
                        BaseRequest<OrgFloorFilter> request = new BaseRequest<OrgFloorFilter>();
                        request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                        request.Data.FloorId = value;
                        IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
                        var response = service.QueryOrgFloor(request);
                        if (response.Data.Count > 0)
                        {
                            result = ((List<OrgFloor>)response.Data).Any(it => it.FloorId != key);
                        }
                        break;
                    }
                case "RoomNo":
                    {
                        BaseRequest<OrgRoomFilter> request = new BaseRequest<OrgRoomFilter>();
                        request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                        request.Data.RoomNo = value;
                        IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
                        var response = service.QueryOrgRoom(request);
                        if (response.Data.Count > 0)
                        {
                            result = ((List<OrgRoom>)response.Data).Any(it => it.RoomNo != key);
                        }
                        break;
                    }
                case "DeptNo":
                    {
                        BaseRequest<DeptFilter> request = new BaseRequest<DeptFilter>();
                        request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                        request.Data.DeptNo = value;
                        IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
                        var response = service.QueryDept(request);
                        if (response.Data.Count > 0)
                        {
                            result = ((List<Dept>)response.Data).Any(it => it.DeptNo != key);
                        }
                        break;
                    }
                case "BedNo":
                    {
                        BaseRequest<BedBasicFilter> request = new BaseRequest<BedBasicFilter>();
                        request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                        request.Data.BedNo = value;
                        IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
                        var response = service.QueryBedBasic(request);
                        if (response.Data.Count > 0)
                        {
                            result = ((List<BedBasic>)response.Data).Any(it => it.BedNo != key);
                        }
                        break;
                    }
                case "RoleName":
                    {
                        BaseRequest<RoleFilter> request = new BaseRequest<RoleFilter>();
                        request.Data.RoleName = value;
                        request.Data.OrgId = p1;
                        IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
                        var response = service.QueryRole(request);
                        if (response.Data.Count > 0)
                        {
                            result = ((List<Role>)response.Data).Any(it => it.RoleId != key);
                        }
                        break;
                    }
                case "CostItemNo":
                    {
                        BaseRequest<CostItemFilter> request = new BaseRequest<CostItemFilter>();
                        request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                        request.Data.CostItemNo = value;
                        ICostManageService service = IOCContainer.Instance.Resolve<ICostManageService>();
                        var response = service.QueryCostItem(request);
                        if (response.Data.Count > 0)
                        {
                            result = ((List<CostItem>)response.Data).Any(it => it.CostItemNo != key);
                        }
                        break;
                    }
            }
            return Ok(result);
        }
    }

    public class ChangePassword
    {
        public string LoginName { get; set; }
        public string ValidateCode { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public string OrgID { get; set; }
    }
}


using KM.Common;
using KMHC.Infrastructure.Security;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
     [RoutePrefix("api/Authentication")]
    public class AuthenticationController : BaseController
    {
        IAuthenticationService authenticationService = IOCContainer.Instance.Resolve<IAuthenticationService>();
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="auth">参数</param>
        public IHttpActionResult Post([FromBody] dynamic auth)
        {
            BaseResponse<LTCUserData> response = new BaseResponse<LTCUserData>();
            if (auth == null)
            {
                response.ResultCode = 0;
                response.IsSuccess = false;
                response.ResultMessage = "输入的参数不正确";
                return Ok(response);
            }
            string uid = auth.uid.Value;
            string pwd = auth.pwd.Value;

            User user = null;
            if (userCheck(uid, pwd, "", ref user))
            {
                LTCUserData clientUserData = new LTCUserData()
                {
                    UserId = user.UserId,
                    LoginName = user.LogonName,
                    EmpNo = user.EmpNo,
                    EmpName = user.EmpName,
                    EmpGroup = user.EmpGroup,
                    JobTitle = user.JobTitle,
                    JobType = user.JobType,
                    OrgId = user.OrgId,
                    RoleId = user.RoleId,
                    RoleType = user.RoleType,
                    SysType = user.SysType,
                    LTCRoleType = user.LTCRoleType,
                    DCRoleType = user.DCRoleType
                };
                response.IsSuccess = true;
                response.ResultCode = 200;
                response.ResultMessage = "登录成功";
                response.Data = clientUserData;

                //生成Token
                var jwtcreated = Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds + 5);
                var payload = new Dictionary<string, dynamic>
                {
                    {"iss",uid},
                    {"iat",jwtcreated}
                };
                string token = JWT.JsonWebToken.Encode(payload, KMHC.SLTC.Business.Entity.Constants.SecretKey, JWT.JwtHashAlgorithm.HS256);
                response.Token = token;
            }
            else
            {
                response.IsSuccess = false;
                response.ResultCode = 0;
                response.ResultMessage = "输入的用户名或密码不正确";
                return Ok(response);
            }

            return Ok(response);
        }

        public bool userCheck(string name, string pwd, string orgId, ref User user)
        {
            IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            BaseRequest<UserFilter> request = new BaseRequest<UserFilter>();
            request.Data.LoginName = name;
            request.Data.Password = pwd;
            request.Data.OrgId = orgId;
            request.Data.CheckLogin = 1001;
            var userList = service.QueryUserExtend(request);
            if (userList.Data.Count > 0)
            {
                user = userList.Data[0];
            }
            return userList.Data.Count > 0;
        }
    }
}

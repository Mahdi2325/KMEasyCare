using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.Infrastructure.Security
{
    public class LTCUserData
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string EmpName { get; set; }
        /// <summary>
        /// 组织结构Id
        /// </summary>
        public string OrgId { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string EmpNo { get; set; }
        /// <summary>
        /// 事业类型
        /// </summary>
        public string EmpGroup { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string JobTitle { get; set; }
        /// <summary>
        /// 职别
        /// </summary>
        public string JobType { get; set; }
        public string Email { get; set; }

        //政府编码
        public string GovId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string[] RoleId { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public string[] RoleType { get; set; }

        public string[] DCRoleType { get; set; }
        public string[] LTCRoleType { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public string[] SysType { get; set; }

        public string CurrentLoginSys { get; set; }
    }
}


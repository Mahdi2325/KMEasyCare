/*
 * 描述:User
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 2/18/2016 12:02:29 PM   Admin            15986707042@163.com    创建 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class User
    {
        public string EmpName { get; set; }
        public string RealName { get; set; }
        public int Age { get; set; }
        public string ImgUrl { get; set; }
        public string Organization { get; set; }
        public int UserId { get; set; }
        public string LogonName { get; set; }
        public string Pwd { get; set; }
        public Nullable<System.DateTime> PwdExpDate { get; set; }
        public Nullable<int> PwdDuration { get; set; }
        public Nullable<System.DateTime> LastLogonDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool? Status { get; set; }
        public string EmpNo { get; set; }
        public string IsOrgCtrl { get; set; }
        public string PwdEnable { get; set; }
        public string OrgId { get; set; }
        public string[] OrgIds { get; set; }
        //机构对应政府编码
        public string GovId { get; set; }
        public string[] GovIds { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string JobTitle { get; set; }
        /// <summary>
        /// 职别
        /// </summary>
        public string JobType { get; set; }

        public string EmpGroup { get; set; }
     
        public string[] RoleId { get; set; }
        public string[] DCRoleType { get; set; }
        public string[] LTCRoleType { get; set; }
        public string[] RoleType { get; set; }
        //系统
        public string[] SysType { get; set; }
        public string Email { get; set; }
    }
}
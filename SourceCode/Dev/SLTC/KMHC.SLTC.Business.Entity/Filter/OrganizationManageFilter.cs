using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class OrganizationFilter
    {
        //机构ID
        public string OrgID { get; set; }
        //机构代码
        public string GroupID { get; set; }
        //机构名称
        public string OrgName { get; set; }
        //机构类型
        public string OrgType { get; set; }
        //负责人
        public string Responsible { get; set; }
        //电话
        public string Tel { get; set; }
        //传真
        public string Fax { get; set; }
        //床位数
        public int? BedCount { get; set; }
        //电子邮件
        public string Email { get; set; }
        //网站
        public string Website { get; set; }
        //状态
        public string Status { get; set; } 
    }

    public class BedBasicFilter
    {
        //床号编号
        public string BedNo { get; set; }
        public string OrgId { get; set; }
        public string KeyWords { get; set; }
        public string RoomNo { get; set; }
        public string BedStatus { get; set; }
    }

    public class OrgFloorFilter
    {
        public string FloorId { get; set; }
        public string FloorName { get; set; }
        public string OrgId{get;set;}
    }

    public class OrgRoomFilter 
    {
        public string OrgId { get; set; }
        public string OrgName{get;set;}
        public string FloorName{get;set;}
        public string RoomNo { get; set; }
        public string RoomName { get; set; }
		public string FloorId { get; set; }
        public int? EmptyBedNumber { get; set; }
        public string Sex { get; set; }
    }

    public class UserFilter
    {
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string EmpNo { get; set; }
        public string OrgId { get; set; }
        public string Findtype { get; set; }
        public int CheckLogin { get; set; }
        public string RoleType { get; set; }
        public string LoginSysType { get; set; }
    }
    public class RoleFilter
    {
        public string[] RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleType { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
        public string OrgId { get; set; }
        public string CurrentLoginSys { get; set; }
        
    }

    public class ModuleFilter
    {
    }

    public class EmployeeFilter
    {
        public string EmpNo { get; set; }
        public string EmpName { get; set; }

        public string EmpGroup { get; set; }

        public string OrgId { get; set; } 
        public int NeedEmp { get; set; }
        public string IdNo { get; set; }
    }

    public class DeptFilter
    {
        public string DeptNo { get; set; }
        public string DeptName { get; set; }
        public string OrgId { get; set; } 
    }

    public class GroupFilter
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string Address { get; set; }
    }
    
    
}






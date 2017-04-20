using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Role
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleType { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool Status { get; set; }
        public List<TreeNode> CheckModuleList { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public string SysType { get; set; }
    }
}






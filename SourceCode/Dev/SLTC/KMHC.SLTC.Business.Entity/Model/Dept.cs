using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Dept
    {
        public string DeptNo { get; set; }
        public string DeptName { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<bool> Status { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }
    }
}






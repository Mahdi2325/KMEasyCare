using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
    public class DC_TaskEmpModel
    {
        public string EmpNo  { get; set; }
        public string EmpName { get; set; }
        public string EmpGroup { get; set; }
        public string JobTitle { get; set; } 
        public string JobType  { get; set; }   
        public string OrgId  { get; set; }
        public string EmpGroupName { get; set; }
        public bool Checked { get; set; }
    }

    public class DC_ReAllocateTaskModel
    {
        public List<DC_TaskEmpModel> empList { get; set; }
        public long ID { get; set; }
        public DateTime? PerformDate { get;set;}
        public string Content { get; set; }
    }

    public class DC_TaskEmpByGroup
    {
        public string EmpGroup { get; set; }
        public string EmpGroupName { get; set; }
        public List<DC_TaskEmpModel> EmpList { get; set; }
    }
}

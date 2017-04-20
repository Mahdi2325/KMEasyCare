using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Filter
{
    public class DC_AssignJobsFilter
    {
        public long ID { get; set; }
        public DateTime? AssignDate { get; set; }
        public DateTime? AssignStartDate { get; set; }
        public DateTime? AssignEndDate { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedName  { get; set; }
        public string Assignee { get; set; }
        public string AssignName { get; set; }
        public bool? RecStatus { get; set; }
        public DateTime? FinishDate { get; set; }
        public bool? NewRecFlag  { get; set; }
        public Nullable<long> FeeNo  { get; set; }
        public string OrgId  { get; set; }
    }
}

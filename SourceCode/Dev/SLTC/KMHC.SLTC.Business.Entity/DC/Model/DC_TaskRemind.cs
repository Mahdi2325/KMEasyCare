using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
   public class DC_TaskRemind
    {
        public long ID { get; set; }
        public Nullable<System.DateTime> AssignDate { get; set; }
        public string AssignedBy  { get; set; }
        public string AssignedName  { get; set; }
        public string Assignee  { get; set; }
        public string AssignName  { get; set; }
        public string Content  { get; set; }
        public Nullable<bool> RecStatus  { get; set; }
        public Nullable<System.DateTime> FinishDate  { get; set; }
        public string UnFinishReason  { get; set; }
        public Nullable<System.DateTime> PerformDate  { get; set; }
        public string URL { get; set; }
        public Nullable<bool> NewRecFlag  { get; set; }
        public Nullable<long> FeeNo  { get; set; }
        public string ResidentName { get; set; }
        public string OrgId  { get; set; }
    }
}

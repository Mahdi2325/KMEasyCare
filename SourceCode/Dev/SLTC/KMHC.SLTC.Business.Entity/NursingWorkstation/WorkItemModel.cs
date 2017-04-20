using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.NursingWorkstation
{
    public class WorkItemModel
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public Nullable<int> Order { get; set; }
        public string Description { get; set; }
    }
}

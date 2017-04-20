using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_TaskGoalsStrategyFilter
    {
        public long EvalPlanId { get; set; }
        public long Id { get; set; }
        public DateTime? RecDate { get; set; }
        public string OrgId { get; set; }
        public int FeeNo { get; set; }
    }
}

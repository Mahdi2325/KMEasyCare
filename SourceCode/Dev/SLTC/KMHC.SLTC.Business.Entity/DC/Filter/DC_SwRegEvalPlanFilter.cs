using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_SwRegEvalPlanFilter
    {
        public string _orgId { get; set; }
        public long _evalplanid { get; set; }
        public long? _feeno { get; set; }
        public string _residentno { get; set; }
        public DateTime? _evaldate { get; set; }
        public int? _number { get; set; }
    }
}

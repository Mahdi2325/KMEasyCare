using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Filter
{
    public class DC_RegCplFilter
    {
        public long FeeNo { get; set; }
        public long Id { get; set; }
        public string EvaluationValue { get; set; }
        public bool FinishFlag { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
   public class CloseFinancialFilter
    {
        public long feeNo { get; set; }
        public DateTime financialCloseTime { get; set; }
        public string type { get; set; }
    }
}

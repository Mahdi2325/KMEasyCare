using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class AdvisoryRegFilter
    {
        public DateTime? ConsultStartTime { get; set; }
        public DateTime? ConsultEndTime { get; set; }
        public string KeyWords { get; set; }
    }
}

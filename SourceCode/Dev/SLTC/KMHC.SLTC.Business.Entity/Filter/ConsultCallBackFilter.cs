using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class ConsultCallBackFilter
    {
        public DateTime? CallBackStartTime { get; set; }
        public DateTime? CallBackEndTime { get; set; }
        public long ConsultRecId { get; set; }
    }
}

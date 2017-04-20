using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class BillV2Filter
    {
        public int? FeeNo { get; set; }

        public DateTime StarDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public partial class RefundMgmtFilter
    {
        public long? FEENO { get; set; }
        public int? STATUS { get; set; }

        public string BILLID{ get; set; }

        public int? REFUNDRECORDID { get; set; }
    }
}

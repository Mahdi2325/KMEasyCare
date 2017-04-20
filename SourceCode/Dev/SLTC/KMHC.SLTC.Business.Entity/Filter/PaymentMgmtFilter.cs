using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public partial class PaymentMgmtFilter
    {
        public long? FEENO { get; set; }
        public int? STATUS { get; set; }

        public int? FEERECORDID { get; set; }

        public int? CHARGERECORDTYPE { get; set; }

        public string BILLID{ get; set; }

        public int? BILLPAYID { get; set; }
    }
}

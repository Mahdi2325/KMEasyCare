using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.BillManagement
{
    public class RESIDENTMONFEE
    {
        public string YearMonth { get; set; }
        public string Name { get; set; }
        public Nullable<int> HospDay { get; set; }
        public Nullable<decimal> NCIPayLevel { get; set; }
        public Nullable<decimal> NCIPaysCale { get; set; }
        public decimal NCIItemTotalCost { get; set; }
        public decimal NCIPay { get; set; }
        public DateTime? INDATE { get; set; }
        public DateTime? OUTDATE { get; set; }
        public DateTime? BALANCESTARTTIME { get; set; }
        public DateTime? BALANCEENDTIME { get; set; }
    }
}

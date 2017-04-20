using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class MonthlyPayLimit
    {
        public int PayLimitID { get; set; }
        public string YearMonth { get; set; }
        public long FeeNO { get; set; }
        public string ResidentssID { get; set; }
        public decimal PayedAmount { get; set; }
        public Nullable<decimal> NCIPayLevel { get; set; }
        public Nullable<decimal> NCIPaysCale { get; set; }
        public string Orgid { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}

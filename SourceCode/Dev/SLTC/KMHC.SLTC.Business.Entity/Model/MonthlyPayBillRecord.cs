using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class MonthlyPayBillRecord
    {
        public int ID { get; set; }
        public string BillID { get; set; }
        public string YearMonth { get; set; }
        public Nullable<long> FeeNO { get; set; }
        public Nullable<decimal> PayedAmount { get; set; }
        public Nullable<System.DateTime> CompStartDate { get; set; }
        public Nullable<System.DateTime> CompEndDate { get; set; }
        public int Status { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}

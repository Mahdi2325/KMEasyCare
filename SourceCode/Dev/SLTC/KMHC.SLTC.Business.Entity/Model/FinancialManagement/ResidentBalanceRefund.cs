using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model.FinancialManagement
{
   public class ResidentBalanceRefund
    {
        public int BALANCEREFUNDID { get; set; }
        public string BALANCEID { get; set; }
        public decimal REFUNDAMOUNT { get; set; }
        public string REASON { get; set; }
        public string OPERATOR { get; set; }

        public string EMPNAME { get; set; }
        public string RECEIVER { get; set; }
        public string PAYMENTTYPE { get; set; }
        public System.DateTime REFUNDTIME { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
    }
}

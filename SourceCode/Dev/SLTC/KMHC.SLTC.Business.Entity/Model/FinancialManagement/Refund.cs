using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model.FinancialManagement
{
    public class Refund
    {
        public int REFUNDRECORDID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string NEWBILLID { get; set; }
        public string REFUNDREASON { get; set; }
        public decimal SELFPAY { get; set; }
        public decimal NCIITEMTOTALCOST { get; set; }
        public decimal NCIPAY { get; set; }
        public decimal NCIITEMSELFPAY { get; set; }
        public decimal REFUNDAMOUNT { get; set; }
        public string COMMENT { get; set; }
        public string OPERATOR { get; set; }

        public string EMPNAME { get; set; }
        public string RECEIVER { get; set; }
        public string PAYMENTTYPE { get; set; }
        public Nullable<System.DateTime> REFUNDTIME { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
    }
}

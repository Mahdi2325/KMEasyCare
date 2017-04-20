using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public partial class PreCharge  
    {
        public int PRECHARGEID { get; set; }
        public string BALANCEID { get; set; }
        public decimal AMOUNT { get; set; }
        public string PAYMENTTYPE { get; set; }
        public string PAYER { get; set; }
        public string RECEIPTNO { get; set; }
        public string OPERATOR { get; set; }
        public Nullable<System.DateTime> PRECHARGETIME { get; set; }

        public string EMPNAME { get; set; }
        public string COMMENT { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class BillV2PAY
    {
        public int BILLPAYID { get; set; }
        public decimal SELFPAY { get; set; }
        public decimal NCIITEMTOTALCOST { get; set; }
        public decimal NCIPAY { get; set; }
        public decimal NCIITEMSELFPAY { get; set; }
        public Nullable<decimal> ACCOUNTBALANCEPAY { get; set; }
        public string OPERATOR { get; set; }
        public string EMPNAME { get; set; }
        public string PAYER { get; set; }
        public string PAYMENTTYPE { get; set; }
        public string INVOICENO { get; set; }
        public long FEENO { get; set; }
        public Nullable<System.DateTime> PAYTIME { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }

        public string OrgId { get; set; }
        public int Stasus { get; set; }
        public decimal? RegScal { get; set; }
    }
}

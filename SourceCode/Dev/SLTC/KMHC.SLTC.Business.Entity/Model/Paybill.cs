namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PayBill
    {
        public long Id { get; set; }
        public long BillId { get; set; }
        public string PayBillNo { get; set; }
        public Nullable<System.DateTime> PayBillTime { get; set; }
        public string Payor { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string InvoiceNo { get; set; }
        public string AccountantCode { get; set; }
        public string RecrodBy { get; set; }
        public Nullable<decimal> Summary { get; set; }
        public Nullable<decimal> Received { get; set; }
        public string BillStatus  { get; set; }
    }
}

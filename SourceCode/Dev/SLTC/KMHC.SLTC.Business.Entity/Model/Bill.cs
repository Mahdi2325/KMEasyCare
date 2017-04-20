namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bill
    {
        public long Id { get; set; }
        public string BillNo { get; set; }
        public string BillType { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public Nullable<System.DateTime> BillEndDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string BillState { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Description { get; set; }
        public int RegNo { get; set; }
        public long FeeNo { get; set; }
        public string OrgId { get; set; }
    }
}

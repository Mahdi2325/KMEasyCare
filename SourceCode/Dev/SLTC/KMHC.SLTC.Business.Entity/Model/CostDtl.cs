namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CostDtl
    {
        public long Id { get; set; }
        public string CostNo { get; set; }
        public string CostItemNo { get; set; }
        public string CostName { get; set; }
        public string ItemType { get; set; }
        public Nullable<System.DateTime> OccurTime { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<bool> SelfFlag { get; set; }
        public string Description { get; set; }
        public int RegNo { get; set; }
        public long FeeNo { get; set; }
        public string OrgId { get; set; }
        public int BillId { get; set; }
    }
}

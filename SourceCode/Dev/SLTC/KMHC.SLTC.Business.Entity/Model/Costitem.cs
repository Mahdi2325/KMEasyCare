namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CostItem
    {
        public int Id { get; set; }
        public string CostItemNo { get; set; }
        public string CostName { get; set; }
        public string ItemType { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string OrgId { get; set; }
    }
}

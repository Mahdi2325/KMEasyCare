namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class FixedCost
    {
        public int Id { get; set; }
        public int CostItemId { get; set; }
        public string CostItemNo { get; set; }
        public string CostName { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Period { get; set; }
        public Nullable<int> RepeatCount { get; set; }
        public Nullable<DateTime> GenerateDate { get; set; }
        public int RegNo { get; set; }
        public long FeeNo { get; set; }
        public string OrgId { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public Nullable<bool> IsEndCharGes { get; set; }
    }
}

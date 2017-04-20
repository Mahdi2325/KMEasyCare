namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;

    public partial class StatisticItem
    {
        public string Category { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Total { get; set; }
    }
}

namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;

    public partial class InfectionSympotm
    {
        public long Id { get; set; }
        public Nullable<long> SeqNo { get; set; }
        public string ItemName { get; set; }
        public string Sympotm { get; set; }
        public Nullable<System.DateTime> OccurDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string ItemNo { get; set; }
    }
}

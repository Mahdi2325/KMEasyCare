namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class LabExamRec
    {
        public long Id { get; set; }
        public Nullable<long> SeqNo { get; set; }
        public string Unit { get; set; }
        public Nullable<System.DateTime> ExamDate { get; set; }
        public string ExamType { get; set; }
        public Nullable<System.DateTime> RptDate { get; set; }
        public string Fungus { get; set; }
        public string ExamResults { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
    }
}

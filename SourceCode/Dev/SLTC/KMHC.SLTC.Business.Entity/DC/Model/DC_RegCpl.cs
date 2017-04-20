using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
    public class DC_RegCpl
    {
        public long SeqNo { get; set; }
        public long FeeNo { get; set; }
        public string RegNo { get; set; }
        public string EmpNo { get; set; }
        public DateTime? StartDate { get; set; }
        public int NeedDays { get; set; }
        public DateTime? TargetDate { get; set; }
        public string MajorType { get; set; }
        public string CpLevel { get; set; }
        public string CpDia { get; set; }
        public long Id { get; set; }
        public string NsDesc { get; set; }
        public string CpReason { get; set; }
        public bool FinishFlag { get; set; }
        public DateTime? FinishDate { get; set; }
        public int TotalDays { get; set; }
        public string CpResult { get; set; }
        public string UnfinishReason { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }    
    }

    public class NSCPLGOAL
    {
        public long Id { get; set; }
        public long SeqNo { get; set; }
        public DateTime? RecDate { get; set; }
        public string CplGoal { get; set; }
        public DateTime? FinishDate { get; set; }
        public long FinishFlag { get; set; }
        public string UnfinishReason { get; set; }

    }

    public class NSCPLACTIVITY
    {
        public long Id { get; set; }
        public long SeqNo { get; set; }
        public DateTime? RecDate { get; set; }
        public string CplActivity { get; set; }
        public long FinishFlag { get; set; }
        public DateTime? FinishDate { get; set; }
        public string UnfinishReason { get; set; }

    }

    public class ASSESSVALUE
    {
        public long Id { get; set; }
        public long SeqNo { get; set; }
        public DateTime? RecDate { get; set; }
        public string ValueDesc { get; set; }
        public string RecordBy { get; set; }
        public string ExecuteBy { get; set; }

    }
}

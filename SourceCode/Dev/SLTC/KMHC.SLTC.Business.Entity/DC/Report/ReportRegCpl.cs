using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Report
{
    public class ReportRegCpl
    {
        public string RegName { get; set; }
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
        public string OrgName { get; set; } 
    }
}

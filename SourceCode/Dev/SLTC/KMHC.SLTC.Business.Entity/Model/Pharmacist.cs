using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Pharmacist
    {
        public long Id { get; set; }
        public long? FeeNo { get; set; }
        public int? RegNo { get; set; }
        public DateTime? EvalDate { get; set; }
        public DateTime? NextEvalDate { get; set; }
        public string EvaluateBy { get; set; }
        public int Interval { get; set; }
        public string EvaluateByName { get; set; }
        public string DiseaseDesc { get; set; }
        public string PipleLineDesc { get; set; }
        public string VitalSigns { get; set; }
        public string M3visitRec { get; set; }
        public string DrugRecords { get; set; }
        public bool YearsOld85 { get; set; }
        public bool Drug9Type { get; set; }
        public bool AdrsFlag { get; set; }
        public bool SpecTypeDrgFlag { get; set; }
        public string SpecTypeDrugDesc { get; set; }
        public bool SpecDrugFlag { get; set; }
        public string SpecDrugDesc { get; set; }
        public string InterAction { get; set; }
        public string Suggestion { get; set; }
        public bool MillsFlag { get; set; }
        public bool DeptVisit3 { get; set; }
        public string NextEvaluateBy { get; set; }
        public string NextEvaluateByName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string MultipleFlag { get; set; }
        public string OrgId { get; set; }
    }
}

using KMHC.SLTC.Business.Entity.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class EvalReportHeader
    {
        public long RecordId { get; set; }
        public string Name { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public string ResidengNo { get; set; }
        public string Area { get; set; }
        public string BedNo { get; set; }
        public string Age { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<DateTime> NextDate { get; set; }
        public string EvaluateBy { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public string QuestionCode { get; set; }
        public string Result { get; set; }
        public decimal? Score { get; set; }
        public string OrgId { get; set; }
        public string Org { get; set; }
        public DateTime? Brithdate { get; set; }
        public string CDTW { get; set; }
        public string NDTW { get; set; }
        public int OneEvaluateTotal { get; set; }
        public int EvaluateTotal { get; set; }
        public IEnumerable<Answer> answer { get; set; }
        public string FloorId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Report
{
    public class Question
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public int? Age { get; set; }

        public long? FeeNo { get; set; }
        public string ResidengNo { get; set; }
        public string BedNo { get; set; }

        public string Area { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CDTW { get; set; }

        public DateTime? NextDate { get; set; }
        public string NDTW { get; set; }

        public string EvaluateBy { get; set; }

        public string Result { get; set; }

        public decimal? Score { get; set; }

        public string OrgId { get; set; }
        public string Org { get; set; }
        public int? QuestionId { get; set; }
        public int OneEvaluateTotal { get; set; }
        public int EvaluateTotal { get; set; }
        public DateTime? Brithdate { get; set; }
    }
}

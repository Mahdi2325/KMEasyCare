using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class PipelineEvalModel
    {
        public long Id { get; set; }
        public long? FeeNo { get; set; }
        public DateTime? EvalDate { get; set; }
        public DateTime? RecentDate { get; set; }
        public int? IntervalDay { get; set; }
        public DateTime? NextDate { get; set; }
        public string State { get; set; }
        public string Operator { get; set; }
        public string PipelineName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
        public int? SeqNo { get; set; }
        public long? Seq { get; set; }
        public string PipelineType { get; set; }
        public string OperatorName { get; set; }
    }
}

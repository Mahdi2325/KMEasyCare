using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class PipelineRecModel
    {
        public int? SeqNo { get; set; }
        public long? FeeNo { get; set; }
        public DateTime? RecordDate { get; set; }
        public string PipelineName { get; set; }
        public string Operator { get; set; }
        public bool? RemoveFlag { get; set; }
        public string RemoveTrain { get; set; }
        public bool? RemovedFlag { get; set; }
        public DateTime? RemoveDate { get; set; }
        public string RemoveReason { get; set; }
        public string RemoveBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string author { get; set; }

        public string OperatorName { get; set; }
        public string RemoveByName { get; set; }
        //管路评估明细
        public List<PipelineEvalModel> PipelineEval { get; set; }
    }
}

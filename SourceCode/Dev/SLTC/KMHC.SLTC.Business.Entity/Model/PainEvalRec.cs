using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class PainEvalRec
    {
        public long SeqNo { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public Nullable<System.DateTime> EvalDate { get; set; }
        public Nullable<System.DateTime> NextEvaldate { get; set; }
        public string NextEvaluateBy { get; set; }
        public string DiagDesc { get; set; }
        public string TransferPart { get; set; }
        public string Coma_e { get; set; }
        public string Coma_m { get; set; }
        public string Coma_v { get; set; }
        public string GCS { get; set; }
        public string Consciousness { get; set; }
        public string PainExpression { get; set; }
        public string PainReaction { get; set; }
        public bool CancelFlag { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        public string CancelReason { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }

        /// <summary>
        /// 下次委托人姓名
        /// </summary>
        public string NextEvaluateByName { get; set; }
        public List<PainBodyPartRec> Detail { get; set; }
    }
}






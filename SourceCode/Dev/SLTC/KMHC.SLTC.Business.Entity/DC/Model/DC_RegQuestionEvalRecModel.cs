using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    /// <summary>
    /// 受托长辈适应程度评估集合
    /// </summary>
    public class RegQuestionEvalRec
    {
        public DC_RegQuestionEvalRecModel RegEvalQuestionRec { get; set; }

        public List<DC_RegEvalQuestionModel> RegEvalQuestion { get; set; }
        public List<DC_RegQuestionDataModel> RegQuestionData { get; set; }

        public DC_QuestionModel Questions { get; set; }
        public List<DC_QuestionItemModel> QuestionItem { get; set; }
    }
    public class DC_RegQuestionEvalRecModel
    {
        public int? EvalRecId { get; set; }
        public float? Score { get; set; }
        public long? FeeNo { get; set; }
        public DateTime? EvalDate { get; set; }
        public string EvalResult { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string OrgId { get; set; }

        public DC_QuestionModel Questions { get; set; }

        public List<DC_RegEvalQuestionModel> RegEvalQuestion { get; set; }

        public List<DC_RegQuestionDataModel> RegQuestionData { get; set; }

        public List<DC_QuestionItemModel> QuestionItem { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class QuestionModel
    {
        public DC_QuestionModel questionModel{get;set;}
                                          
        public List<DC_QuestionItemModel> QuestionItem { get; set; }

        public List<DC_QuestionValueModel> QuestionValue { get; set; }

        public List<DC_RegQuestionDataModel> QuestionItemData { get; set; }
    }
    public class DC_QuestionModel
    {
        public int? EVALRECID { get; set; }
        public int QUESTIONID { get; set; }
		public string QUESTIOCODE { get; set; }
        public string QUESTIONNAME { get; set; }
        public int? SHOWNUMBER { get; set; }
        public bool ISSHOW { get; set; }
        public string QUESTIONDESC { get; set; }
        public string ORGID { get; set; }
		public string QUESTIONCODE { get; set; }
        public int? FEENO { get; set; }
        public float? SCORE { get; set; }
        public string EVALRESULT { get; set; }

        public List<DC_QuestionItemModel> QuestionItem { get; set; }
        public List<DC_QuestionValueModel> QuestionValue { get; set; }
        public List<DC_RegQuestionDataModel> QuestionItemData { get; set; }
       // public List<DC_RegEvalQuestionModel> RegEvalQuestion { get; set; }
        //public List<DC_RegQuestionDataModel> QuestionItemData { get; set; }

    }
}

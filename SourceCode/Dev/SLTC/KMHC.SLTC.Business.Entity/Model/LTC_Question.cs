using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class LTC_Question
    {
        #region 基本属性
        //自增长ID
        public int Id { get; set; }
        //评估量表标示ID
        public Nullable<int> QuestionId { get; set; }
        //评估量表名称
        public string QuestionName { get; set; }
        //序列号
        public Nullable<int> ShowNumber { get; set; }
        //是否显示
        public Nullable<bool> IsShow { get; set; }
        //评估量表描述
        public string QuestionDesc { get; set; }
        //量表类别
        public Nullable<int> CategoryId { get; set; }
        //量表Code
        public string Code { get; set; }
        //是否计算总分
        public Nullable<bool> ScoreFlag { get; set; }
        //机构ID
        public string OrgId { get; set; }
        #endregion
        #region 扩展属性
        //评估问题
        public List<LTC_MakerItem> MakerItem { get; set; }
        //评估结果
        public List<LTC_QuestionResults> QuestionResults { get; set; }

        public Nullable<int> ExportQuestionId { get; set; }
        public Nullable<bool> Status { get; set; }
        #endregion
    }

    public class EvalTempSetMainModel
    {
        public string OrgId { get; set; }
        public List<EvalTempSetModel> Items { get; set; }
    }

    public class EvalTempSetModel
    {
        public Nullable<int> CategoryId { get; set; }
        public List<LTC_Question> Items { get; set; }
    }
}


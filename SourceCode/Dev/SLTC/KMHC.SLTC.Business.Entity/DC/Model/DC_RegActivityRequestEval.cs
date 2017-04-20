using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
    public class DC_RegActivityRequestEval
    {
        public long Id { get; set; }
        public long FeeNo { get; set; }
        public string RegNo { get; set; }
        public DateTime? InDate { get; set; }
        public DateTime? EvalDate { get; set; }
        public int Interval { get; set; }
        public DateTime? NextevalDate { get; set; }
        public string RegName { get; set; }
        public string Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        //教育程度
        public string Education { get; set; }
        //婚姻状况
        public string MerryState { get; set; }
        //退休前职业
        public string Profession { get; set; }
        //沟通语言
        public string Language { get; set; }
        //专长
        public string Skill { get; set; }
        //兴趣
        public string Interesting { get; set; }
        //视觉
        public string Visualacuity { get; set; }
        //听觉
        public string ListeningState { get; set; }
        //妄想
        public string Delusion { get; set; }
        //其他幻觉
        public string Visualillusion { get; set; }
        //MMSE 分数
        public string MmseScore { get; set; }
        //MMSE 项目
        public string MmseItemResult { get; set; }
        //MMSE 评估结果
        public string MmseResult { get; set; }
        //
        public string AdlScore { get; set; }
        //
        public string AdlItemResult { get; set; }
        //
        public string AdlResult { get; set; }
        //
        public string IadlScore { get; set; }
        //
        public string IadlItemResult { get; set; }
        //
        public string IadlResult { get; set; }
        //
        public string GdsScore { get; set; }
        //
        public string GdsItemResult { get; set; }
        //
        public string GdsResult { get; set; }
        //情绪
        public string EmotionState { get; set; }
        //问题行为
        public string ProblemBehavior { get; set; }
        //人际
        public string Interpersonal { get; set; }
        //自我
        public string Self { get; set; }
        //增进/促进
        public string Promotion { get; set; }
        //增进/促进--实际策略
        public string PromotionStrategy { get; set; }
        //维持
        public string Preserve { get; set; }
        //维持--实际策略
        public string PreserveStrategy { get; set; }
        //延缓/减少
        public string Ease { get; set; }
        //延缓/减少--实际策略
        public string EaseStrategy { get; set; }
        //
        public bool? DelFlag { get; set; }
        public DateTime? DelDate { get; set; }
        public string OrgId { get; set; }    
    }
}


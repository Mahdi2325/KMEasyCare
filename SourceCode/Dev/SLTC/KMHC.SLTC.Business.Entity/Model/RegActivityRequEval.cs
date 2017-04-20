using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class RegActivityRequEval
    {
        public long Id { get; set; }

        public Nullable<long> regfeeno { get; set; }

        public Nullable<DateTime> RegEvalDate { get; set; }
        public DateTime InDate { get; set; }
        public DateTime EvalDate { get; set; }
        /// <summary>
        /// 楼层编号
        /// </summary>
        public string floorId { get; set; }
        /// <summary>
        /// 社工编号
        /// </summary>
        public string Carer { get; set; }
        /// <summary>
        /// 社工姓名
        /// </summary>
        public string CarerName { get; set; }
        //视觉
        public string Vision { get; set; }
        //嗅觉
        public string Smell { get; set; }
        //触觉
        public string Sensation { get; set; }
        //味觉
        public string Taste { get; set; }
        //听觉
        public string Hearing { get; set; }
        //上肢
        public string Upperlimb { get; set; }
        //下肢
        public string Lowerlimb { get; set; }
        //幻觉
        public string Hallucination { get; set; }
        //妄想
        public string Delusion { get; set; }
        //注意力
        public string Attention { get; set; }
        //定向感
        public string Directionsense { get; set; }
        //理解力
        public string Comprehension { get; set; }
        //记忆力
        public string Memory { get; set; }
        //表达
        public string Expression { get; set; }
        //其他叙述
        public string Othernarrative { get; set; }
        //情绪
        public string Emotion { get; set; }
        //自我概念
        public string Self { get; set; }
        //行为内容
        public string Behaviorcontent { get; set; }
        //行为频率
        public string Behaviorfreq { get; set; }
        //活动参与意见
        public string Activity { get; set; }
        //已会谈个案
        public string Talkedwilling { get; set; }
        //无法会谈个案
        public string Nottalked { get; set; }
        //文康休闲活动
        public string Artactivity { get; set; }
        //辅料性活动
        public string Aidsactivity { get; set; }
        //重症区活动
        public string Severeactivity { get; set; }
        //已会谈个案-无意愿
        public string Talkednowilling { get; set; }
        //
        public long FeeNo { get; set; }
        //
        public int RegNo { get; set; }
        //
        public string OrgId { get; set; }

        public string FeeName { get; set; }
        public int Directman { get; set; }
        public int Directtime { get; set; }
        public int Directaddress { get; set; }
        public int Memoryflag { get; set; }  
    }
}


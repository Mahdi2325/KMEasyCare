using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class LTC_MakerItem
    {
        #region 基本属性
        //
        public int MakerId { get; set; }
        //问题名称
        public string MakeName { get; set; }
        //序列号
        public Nullable<int> ShowNumber { get; set; }
        //是否显示
        public Nullable<bool> IsShow { get; set; }
        //
        public Nullable<int> QuestionId { get; set; }
        //数据类型
        public string DataType { get; set; }
        //
        public Nullable<int> LimitedId { get; set; }
        //题目分类
        public string Category { get; set; }
        #endregion
        #region 扩展属性
        //问题答案
        public IList<LTC_MakerItemLimitedValue> MakerItemLimitedValue { get; set; }
        #endregion
    }
}


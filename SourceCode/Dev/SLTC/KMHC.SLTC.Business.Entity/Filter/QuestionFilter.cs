using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class QuestionFilter
    {
        //评估量表名称
        public string Questionname { get; set; }
        //评估量表描述
        public string QuestionDesc { get; set; }
        //机构ID
        public string OrgId { get; set; }

        //是否显示
        public Nullable<bool> IsShow { get; set; }
    }
    public class MakerItemFilter
    {
        public Nullable<int> QuestionId { get; set; }
    }
    public class QuestionResultsFilter
    {
        public Nullable<int> QuestionId { get; set; }
    }
    public class MakerItemLimitedValueFilter
    {
        public Nullable<int> QuestionId { get; set; }
    }
}


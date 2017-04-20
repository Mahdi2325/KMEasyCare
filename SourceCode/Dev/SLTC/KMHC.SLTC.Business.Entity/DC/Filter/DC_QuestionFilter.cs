using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_QuestionFilter
    {
        public int QUESTIONID { get; set; }
        public string QUESTIONNAME { get; set; }
        public int? SHOWNUMBER { get; set; }
        public bool ISSHOW { get; set; }
        public string QUESTIONDESC { get; set; }
        public string ORGID { get; set; }
    }
}

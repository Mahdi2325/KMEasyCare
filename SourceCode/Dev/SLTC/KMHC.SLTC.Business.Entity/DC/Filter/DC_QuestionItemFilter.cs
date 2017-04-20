using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_QuestionItemFilter
    {
        public int ITEMID { get; set; }
        public int? QUESTIONID { get; set; }
        public string ITEMNAME { get; set; }
        public int? SHOWNUMBER { get; set; }
        public string DESCRIPTION { get; set; }

        public int? FEENO { get; set; }
        public int? EVALRECID { get; set; }
    }
}

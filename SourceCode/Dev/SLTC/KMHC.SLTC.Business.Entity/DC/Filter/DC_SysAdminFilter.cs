using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class DC_TeamActivitydtlFilter
    {
        public long? SEQNO { get; set; }
        public string TITLENAME { get; set; }
        public string ORGID { get; set; }

        public string ACTIVITYCODE { get; set; }
    }


    public class DC_COMMFILEFilter
    {
        public string ITEMTYPE { get; set; }
        public string TYPENAME { get; set; }
        public string SUBITEMNAME { get; set; }//子项名称和编号 
        public string MODIFYFLAG { get; set; }
        public string ORGID { get; set; }

    }
}

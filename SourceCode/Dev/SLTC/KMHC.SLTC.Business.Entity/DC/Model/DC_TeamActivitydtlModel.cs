using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//活动项目明细表zhongyh
namespace KMHC.SLTC.Business.Entity
{
    public class DC_TeamActivitydtlModel
    {

        public int ID { get; set; }
        public Nullable<int> SEQNO { get; set; }
        public string TITLENAME { get; set; }
        public string ITEMNAME { get; set; }

        public string ACTIVITYCODE { get; set; }

        public string ORGID { get; set; }
    }
}

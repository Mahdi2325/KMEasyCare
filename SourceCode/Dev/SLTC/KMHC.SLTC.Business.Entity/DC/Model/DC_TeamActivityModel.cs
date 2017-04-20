using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
   public class DC_TeamActivityModel
    {

        public int ID { get; set; }
        public int SEQNO { get; set; }
        public string ACTIVITYCODE { get; set; }
        public string ACTIVITYNAME { get; set; }
        public string ORGID { get; set; } 
        public string TITLENAME { get; set; }
        public string ITEMNAME { get; set; }


    }
}

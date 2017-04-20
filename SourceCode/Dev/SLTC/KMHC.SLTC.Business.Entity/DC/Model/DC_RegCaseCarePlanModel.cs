using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{

   
  public class DC_RegCaseCarePlanModel
    {
        
        public long ID { get; set; }
        public string QUESTIONTYPE { get; set; }
        public string GOAL { get; set; }
        public string ACTIVITY { get; set; }
        public string MAJORTYPE { get; set; }
        public string TRACEDESC { get; set; }
        public Nullable<long> SEQNO { get; set; }
      //DC_REGCASECAREPLAN
    }
}

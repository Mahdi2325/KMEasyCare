using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Filter
{
    public class DC_ResidentFilter
    {
        public string RegNo { get; set; }
        public long? FeeNo { get; set; }
        //学员号
        public string ResidentNo { get; set; }
        public string StationCode { get; set; }
        public string IpdFlag { get; set; }
        public string IdNo { get; set; }
        public string OrgId { get; set; }
        public string ResidentName { get; set; }
    }
}

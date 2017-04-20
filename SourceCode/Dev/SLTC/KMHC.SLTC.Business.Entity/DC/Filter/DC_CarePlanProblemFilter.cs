using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Filter
{
    public class DC_CarePlanProblemFilter
    {
        public int CpNo { get; set; }
        public string OrgId { get; set; }
        public string MajorType { get; set; }
    }


    public class DC_CarePlanActivityFilter
    {
        public int CaNo { get; set; }
        public int CpNo { get; set; }
    }

    public class DC_CarePlanDiaFilter
    {
        public int Id { get; set; }
        public int CpNo { get; set; }
    }
}

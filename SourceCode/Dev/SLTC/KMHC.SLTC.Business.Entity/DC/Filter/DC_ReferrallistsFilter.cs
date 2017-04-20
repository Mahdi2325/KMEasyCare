using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_ReferrallistsFilter
    {
        public int Id { get; set; }
        public string RegNo { get; set; }
        public long? FeeNo { get; set; }
        //学员号
        public string ResidentNo { get; set; }
        public string OrgId { get; set; }
    }
}

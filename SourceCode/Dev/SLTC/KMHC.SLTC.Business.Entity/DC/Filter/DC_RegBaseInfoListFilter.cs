using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
    public class DC_RegBaseInfoListFilter
    {
        public long Id { get; set; }
        public DateTime? RecordDate { get; set; }
        public int Cnt { get; set; }
        public long FeeNo { get; set; }
        public string RegNo { get; set; }
        public string OrgId { get; set; }        
    }
}

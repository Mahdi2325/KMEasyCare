using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_LifeHistoryFilter
    {
        public int Id { get; set; }
        public string OrgId { get; set; }
        public long? FeeNo { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class NSMedicalMaterialFilter
    {
        public int? MaterialId { get; set; }
        public string KeyWord { get; set; }
        public string MCMaterialCode { get; set; }
        public int? Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class PackageRelatedFilter
    {
        public string Name { get; set; }
        //Add By Duke On 20170117
        public string keyWord { get; set; }
        public long FeeNO { get; set; }
    }
}

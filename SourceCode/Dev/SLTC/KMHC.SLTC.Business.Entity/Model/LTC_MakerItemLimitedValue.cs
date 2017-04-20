using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class LTC_MakerItemLimitedValue
    {
        public int LimitedValueId { get; set; }
        public decimal LimitedValue { get; set; }
        public string LimitedValueName { get; set; }
        public int ShowNumber { get; set; }
        public bool IsDefault { get; set; }
        public int LimitedId { get; set; }  
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class MeasureItem
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<float> Upper { get; set; }
        public Nullable<float> Lower { get; set; }
    }
}

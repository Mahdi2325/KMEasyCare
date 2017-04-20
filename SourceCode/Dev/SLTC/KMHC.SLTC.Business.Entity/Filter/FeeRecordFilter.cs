using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
   public class FeeRecordFilter
    {
        public int FeeNo { get; set; }

        public DateTime SDate { get; set; }

        public DateTime EDate { get; set; }

        public string TaskStatus { get; set; }
    }
}

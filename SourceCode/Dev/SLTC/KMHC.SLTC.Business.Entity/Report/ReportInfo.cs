using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Report
{
   public class ReportInfo
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string FLOORID { get; set; }
        public Nullable<DateTime> EVALDATE { get; set; }
        public string CLASSTYPE { get; set; }
    }
}

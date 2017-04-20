using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_RegLifeQualityEvalFilter
    {
        public int _id { get; set; }
        public long? _feeno { get; set; }
        public string _regno { get; set; }
        public string _orgid { get; set; }
        public DateTime? _createdate { get; set; }
    }
}

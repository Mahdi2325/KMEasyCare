using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_EvalQeustionModel
    {
        public decimal? ADL
        {
            get;
            set;
        }
        public decimal? IADL { get; set; }
        public decimal? MMSE { get; set; }
        public decimal? GDS { get; set; }
        public string ORGID { get; set; }
    }
}

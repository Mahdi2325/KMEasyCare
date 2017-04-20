using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_EvalQeustionFilter
    {
        public decimal? ADL
        {
            get;
            set;
        }
        public decimal? IADL { get; set; }
        public decimal? MMSE { get; set; }
        public string ORGID { get; set; }
        public int FEENO { get; set; }
    }
}

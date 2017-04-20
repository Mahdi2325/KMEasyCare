using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class LTC_QuestionResults
    {
        public int ResultId { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public Nullable<decimal> LowBound { get; set; }
        public Nullable<decimal> UpBound { get; set; }
        public string ResultName { get; set; }    
    }
}

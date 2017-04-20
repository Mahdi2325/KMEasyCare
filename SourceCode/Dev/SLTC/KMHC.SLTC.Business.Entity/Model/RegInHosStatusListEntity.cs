using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class RegInHosStatusListEntity
    {
        public string Name { get; set; }
        public string IdNo { get; set; }
        public string IpdFlag { get; set; }
        public DateTime? InDate { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal? LeHour { get; set; }               
        
    }
}

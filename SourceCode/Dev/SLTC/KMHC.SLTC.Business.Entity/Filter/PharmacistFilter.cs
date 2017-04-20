using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class PharmacistFilter
    {
        public long? FeeNo { get; set; } 
        public long? RegNo { get; set; }
        public int Id { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public long TotalRecords { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{

    public class CarePlansFilter
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public int? RegNO { get; set; }
        public string Date { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public long TotalRecords { get; set; }
        public string Category { get; set; }
        public string LevelPR { get; set; }
    }

    public class CarePlanDetailFilter
    {
        public long FeeNo { get; set; }
    }

    public class CarePlanRecFilter
    {
        public long? FeeNo { get; set; }
        public DateTime? SDate { get; set; }

        public DateTime? EDate { get; set; }
    }
}

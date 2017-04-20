using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
   public class NursingFilter
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ItemType { get; set; }
        public string  residengno { get; set; }
        public int? RegNO { get; set; }
        public long? FeeNo { get; set; }
        public string Sex { get; set; }
        public string Date { get; set; }
        public int QuestionId { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public long TotalRecords { get; set; }
    }
}

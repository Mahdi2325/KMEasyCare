using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.TSG
{
   public class TsgAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderSeq { get; set; }
        public string Creater { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        public string Updater { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        public Nullable<bool> Status { get; set; }   
    }
}

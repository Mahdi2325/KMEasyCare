using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.TSG
{
    public class TsgAnswerDtl
    {
        public int Id { get; set; }
        public int AnswerId { get; set; }
        public string Name { get; set; }
        public string AnsdtlImageUrl { get; set; }
        public string Description { get; set; }
        public int OrderSeq { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}

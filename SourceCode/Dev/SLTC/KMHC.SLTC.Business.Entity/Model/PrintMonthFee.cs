using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class PrintMonthFee
    {
        public int Index { get; set; }
        public decimal NCIPayLevel { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal NCIPay { get; set; }
        public int HospDay { get; set; }
        public long? FeeNo { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string ResidentSSId { get; set; }
        public string BrithPlace { get; set; }
        public DateTime? InDate { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? CertStartTime { get; set; }
        public DateTime? EvaluationTime { get; set; }
        public DateTime? ApplyHosTime { get; set; }
        public string CareTypeId { get; set; }
        public string RsStatus { get; set; }
        public string DiseaseDiag { get; set; }
        public string yearMonthArr { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ResidentMonFee
    {
        public string YearMonth { get; set; }
        public string ResidentName { get; set; }
        public string ResidentSSId { get; set; }
        public DateTime HospEntryDate { get; set; }
        public DateTime HospDisChargeDate { get; set; }
        public int HospDay { get; set; }
        public decimal NCIPayLevel { get; set; }
        public decimal NCIPayScale { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal NCIPay { get; set; }
        public long FeeNo { get; set; }
        public string CertNo { get; set; }
        public string CREATEBY { get; set; }
    }
}

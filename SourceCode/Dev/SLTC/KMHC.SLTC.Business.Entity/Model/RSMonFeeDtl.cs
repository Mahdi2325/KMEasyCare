using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class RSMonFeeDtl
    {
        public string FEENAME { get; set; }
        public int FEETYPE { get; set; }
        public string MCCODE { get; set; }
        public decimal UNITPRICE { get; set; }
        public decimal QTY { get; set; }
        public decimal AMOUNT { get; set; }
        public DateTime TAKETIME { get; set; }
        public string OPERATORNAME { get; set; }
        public long FEENO { get; set; }
        public bool ISNCIITEM { get; set; }
        public string CREATEBY { get; set; }
        public string UPDATEBY { get; set; }
    }
}

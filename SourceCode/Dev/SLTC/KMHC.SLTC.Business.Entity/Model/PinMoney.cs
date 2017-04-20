using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class PinMoney
    {
        public int Id { get; set; }
        public string IeNo { get; set; }
        public string ItemSummary { get; set; }
        public string IeTpye { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<System.DateTime> IeDate { get; set; }
        public Nullable<System.DateTime> FactDate { get; set; }
        public string Description { get; set; }
        public int RegNo { get; set; }
        public long FeeNo { get; set; }
        public string OrgId { get; set; }
    }
}

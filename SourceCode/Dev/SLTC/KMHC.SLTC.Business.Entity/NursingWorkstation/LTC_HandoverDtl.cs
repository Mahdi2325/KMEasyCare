using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.NursingWorkstation
{

    public class OverallUU
    {
        public string Flag { get; set; }
        public string Type { get; set; }
    }

    public class LTC_HandoverDtl
    {
        public long Id { get; set; }
        public long RecordId { get; set; }
        public Nullable<long> Feeno { get; set; }
        public string ItemCode { get; set; }
        public Nullable<int> Order { get; set; }
        public string Content { get; set; }
        public string RecordBy { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}

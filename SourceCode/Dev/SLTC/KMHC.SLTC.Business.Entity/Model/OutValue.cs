using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class OutValueModel
    {
        public long OutNo { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<System.DateTime> RecDate { get; set; }
        public string ClassType { get; set; }
        public string OutType { get; set; }
        public Nullable<int> OutValue { get; set; }
        public string CommDesc { get; set; }
        public string RecordBy { get; set; }
        public string RecordNameBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string OrgId { get; set; }
    }
}






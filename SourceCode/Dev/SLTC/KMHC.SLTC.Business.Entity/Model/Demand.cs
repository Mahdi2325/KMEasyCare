using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Demand
    {
        public int Id { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string RegName { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public string DemandType { get; set; }
        public string Content { get; set; }
        public string ExecuteBy { get; set; }
        public string ExecutorName { get; set; }
        public string FinishFlag { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public string UnFinishReason { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }

        public string OrgId { get; set; }
    }
}






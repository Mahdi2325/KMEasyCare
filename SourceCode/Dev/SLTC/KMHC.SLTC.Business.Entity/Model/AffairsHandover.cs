using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class AffairsHandover
    {
        public int Id { get; set; }
        public string ClassType { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public string RecorderName { get; set; }
        public string RecordBy { get; set; }
        public Nullable<System.DateTime> ExecuteDate { get; set; }
        public string ExecutiveName { get; set; }
        public string ExecuteBy { get; set; }
        public bool? FinishFlag { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public string UnFinishReason { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
    }
}






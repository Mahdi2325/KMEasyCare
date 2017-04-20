using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NursingRec
    {
        public long Id { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public string ClassType { get; set; }
        public string Content { get; set; }
        public string RecordBy { get; set; }
        public string RecordNameBy { get; set; }

        /// <summary>
        /// 是否打印                  -- modify by Amaya 
        /// </summary>
        public Nullable<bool> PrintFlag { get; set; }
        /// <summary>
        /// 是否同步录入交班           -- Add by Amaya
        /// </summary>
        public Nullable<bool> SynchronizeFlag { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
        public Nullable<bool> IsSave { get; set; }
       
    }

    public class NursingRecList
    {
        public List<NursingRec> list { get; set; }
    }
}






using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ConstraintRec
    {
        public long SeqNo { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string RecordBy { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public string ClassType { get; set; }
        public Nullable<int> Days { get; set; }
        public string Reason { get; set; }
        public string ExecReason { get; set; }
        public string ConstraintWay { get; set; }
        public string BodyPart { get; set; }
        public string ConstraintWayCnt { get; set; }
        public bool CancelFlag { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        public string Duration { get; set; }
        public string CancelReason { get; set; }
        public string CancelExecBy { get; set; }
        public bool Cancel24Flag { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }

        /// <summary>
        /// 执行人员姓名
        /// </summary>
        public string RecordByName { get; set; }

        /// <summary>
        /// 移除执行人员姓名
        /// </summary>
        public string CancelExecByName { get; set; }

        public List<ConstrainsBeval> Detail { get; set; }
    
    }
}







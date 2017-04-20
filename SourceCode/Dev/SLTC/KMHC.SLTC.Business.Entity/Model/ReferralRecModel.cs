using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ReferralRecModel
    {
        public long Id { get; set; }
        public long? FeeNo { get; set; }
        public int? RegNo { get; set; }
        public DateTime? RecDate { get; set; }
        public string UnitName { get; set; }
        public string UnitContactor { get; set; }
        public string UnitAddress { get; set; }
        public string UnitTel { get; set; }
        public string Reason { get; set; }
        public string QuestionSummary { get; set; }
        public string ServiceSummary { get; set; }
        public string Suggestion { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public DateTime? ReplyDate { get; set; }
        public string ReplyDesc { get; set; }
        public string ReplyType { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
    }
}

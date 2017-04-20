using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    /// <summary>
    /// 院民权益/申诉
    /// </summary>
    public class ComplainRecModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long Id;
        /// <summary>
        /// 收费编号
        /// </summary>
        public long? FeeNo;
        /// <summary>
        /// 病历号
        /// </summary>
        public int? RegNo { get; set; }
        public DateTime? RecDate { get; set; }
        public string QuestionDesc { get; set; }
        public string ProcessBy { get; set; }
        public string ProcessName { get; set; }
        public string QuestionType { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string Status { get; set; }
        public string Results { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
    }
}


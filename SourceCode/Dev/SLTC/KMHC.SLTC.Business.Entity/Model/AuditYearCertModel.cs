using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class AuditYearCertModel
    {
        /// <summary>
        /// 资格申请id
        /// </summary>
        public int? CertAppcertid { get; set; }

        public int? HospAppcertid { get; set; }

        public int? AppHospid { get; set; }
        /// <summary>
        /// 资格申请姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 资格申请年龄
        /// </summary>             
        public int Age { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNo { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        public string SsNo { get; set; }

        public string NsID { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        ///审批状态: 
        ///0：未提交   （数据保存，但尚未提交）
        ///1：已撤回   （提交后，下一级机构尚未审批，撤回）
        ///3: 待审核    （已提交，待上一级机构审核）
        ///6：审核通过 
        ///9：审核不通过
        ///11：重新审核
        /// </summary>
        public int CertStatus { get; set; }
        /// <summary>
        /// 入院申请审核状态
        /// </summary>
        public int HospStatus { get; set; }
        /// <summary>
        /// 年审状态 0:启用 1:禁用
        /// </summary>
        public int YearCertStatus { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Valid { get; set; }

        public DateTime Entrytime { get; set; }

        public string CertNo { get; set; }

        public string Reason { get; set; }

        public DateTime Certstarttime { get; set; }

        public DateTime Certexpiredtime { get; set; }

        public int Caretypeid { get; set; }

        public decimal NCIpaylevel { get; set; }

        public decimal NCIpayscale { get; set; }

        public string NsNo { get; set; }
    }
}

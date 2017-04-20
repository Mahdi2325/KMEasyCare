using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class SubsidyFilter
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
        public int? RegNo;
        /// <summary>
        /// 补助项名称
        /// </summary>
        public string ItemName;
        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime? ApplyDate;
        /// <summary>
        /// 下一次申请日期
        /// </summary>
        public DateTime? NextapplyDate;
        /// <summary>
        /// 备注内容
        /// </summary>
        public string Description;
        /// <summary>
        /// 申请人
        /// </summary>
        public string ApplyBy;
        /// <summary>
        /// 下次受托申请人
        /// </summary>
        public string NextApplyBy;
        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime? Createdate;
        /// <summary>
        /// 记录创建人
        /// </summary>
        public string CreateBy;
        /// <summary>
        /// 机构ID
        /// </summary>
        public string OrgId;
    }
}

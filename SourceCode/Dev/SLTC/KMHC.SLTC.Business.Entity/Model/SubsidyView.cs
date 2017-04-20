using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KMHC.SLTC.Business.Entity;
/*
 * 描述:SubsidyRec  补助申请
 * 修订历史
 * 日期               修改人         Email                               内容
 * 08/03/2016         Deniis         Dennisyang@kmhealthcloud.com        创建
 */

namespace KMHC.SLTC.Business.Entity.Model
{
    public class SubsidyView
    { 

        /// <summary>
        /// 主键ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 收费编号
        /// </summary>
        public long? FeeNo { get; set; }
        /// <summary>
        /// 病历号
        /// </summary>
        public int? RegNo { get; set; }
        /// <summary>
        /// 补助项名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime? ApplyDate { get; set; }
        /// <summary>
        /// 下一次申请日期
        /// </summary>
        public DateTime? NextApplyDate { get; set; }
        /// <summary>
        /// 备注内容
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string ApplyBy { get; set; }
        public string ApplyByName { get; set; }
        /// <summary>
        /// 下次受托申请人
        /// </summary>
        public string NextApplyBy { get; set; }
        public string NextApplyByName { get; set; }
        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 记录创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public string OrgId { get; set; }
    }
}

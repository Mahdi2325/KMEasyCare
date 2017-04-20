using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ResourceLinkModel
    {
        public long Id { get; set; }
        /// <summary>
        /// 收费编号
        /// </summary>
        public long? FeeNo { get; set; }
        /// <summary>
        /// 病历号
        /// </summary>
        public int? FegNo { get; set; }
        /// <summary>
        /// 负责人员
        /// </summary>
        public string RecordBy { get; set; }

        public string RecordByName { get; set; }
        /// <summary>
        /// 首次联系日期
        /// </summary>
        public DateTime? ContactDate { get; set; }
        /// <summary>
        /// 连接完成日期
        /// </summary>
        public DateTime? FinishDate { get; set; }
        /// <summary>
        /// 需求类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 需求名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 需求评估结果
        /// </summary>
        public string EvalResult { get; set; }
        /// <summary>
        /// 提供单位名称
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 连接情形
        /// </summary>
        public string ResourceStatus { get; set; }
        /// <summary>
        /// 未连接原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 资源连接时机
        /// </summary>
        public string RegState { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 记录创建日期
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


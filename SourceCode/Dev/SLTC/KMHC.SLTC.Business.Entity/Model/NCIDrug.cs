using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NCIDrug
    {
        /// <summary>
        /// 医保药品编码
        /// </summary>
        public string DrugCode { get; set; }
        /// <summary>
        /// 医保标准分类码
        /// </summary>
        public string MCRuleId { get; set; }
        /// <summary>
        /// 药品通用中文名称
        /// </summary>
        public string CNName { get; set; }
        /// <summary>
        /// 药品通用英文名称
        /// </summary>
        public string ENName { get; set; }
        /// <summary>
        /// 药品类型
        /// </summary>
        public string DrugType { get; set; }
        /// <summary>
        /// 医保类型
        /// </summary>
        public string MCType { get; set; }
        /// <summary>
        /// 国药准字
        /// </summary>
        public string SFDAApprovalNo { get; set; }
        /// <summary>
        /// 处方标志
        /// </summary>
        public bool IsPrescription { get; set; }
        /// <summary>
        /// 国产进口标志
        /// </summary>
        public bool IsChinaProduct { get; set; }
        /// <summary>
        /// 产地
        /// </summary>
        public string Orgin { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Spec { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Units { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public Nullable<decimal> Unitprice { get; set; }
        /// <summary>
        /// 剂型
        /// </summary>
        public string Form { get; set; }
        /// <summary>
        /// 转换比
        /// </summary>
        public Nullable<int> ConversionRatio { get; set; }
        /// <summary>
        /// 最小包装剂量
        /// </summary>
        public string MinPackage { get; set; }
        /// <summary>
        /// 标准用量
        /// </summary>
        public Nullable<int> StandardUsage { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// 用药频率
        /// </summary>
        public string Frequency { get; set; }
        /// <summary>
        /// 用药途径
        /// </summary>
        public string DrugUsageMode { get; set; }
        /// <summary>
        /// 用法用量
        /// </summary>
        public string DrugUsage { get; set; }
        /// <summary>
        /// 不良反应
        /// </summary>
        public string AdversereAction { get; set; }
        /// <summary>
        /// 注意事项
        /// </summary>
        public string Attention { get; set; }
        /// <summary>
        /// 老年患者注意事项
        /// </summary>
        public string AttentionOldMan { get; set; }
        /// <summary>
        /// 指导单价
        /// </summary>
        public Nullable<decimal> GuidePrice { get; set; }
        /// <summary>
        /// 最高单价
        /// </summary>
        public Nullable<decimal> MaxPrice { get; set; }
        /// <summary>
        /// 护理险每月最大用量
        /// </summary>
        public Nullable<int> NcimonthlyMaxUsage { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 拼音助记码
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 修订日期
        /// </summary>
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> CreateTime { get; set; }
        /// <summary>
        /// 更新人Id
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}

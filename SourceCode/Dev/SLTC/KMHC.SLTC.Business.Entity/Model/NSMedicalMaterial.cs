using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NSMedicalMaterial
    {
        /// <summary>
        /// 耗材ID
        /// </summary>
        public int MaterialId { get; set; }
        /// <summary>
        /// 收费类型ID
        /// </summary>
        public string ChargeTypeId { get; set; }
        /// <summary>
        /// 会计科目ID
        /// </summary>
        public string AccountingId { get; set; }
        /// <summary>
        /// 定点服务机构ID
        /// </summary>
        public string NSId { get; set; }
        /// <summary>
        /// 医保标准分类码
        /// </summary>
        public string MCRuleId { get; set; }
        /// <summary>
        /// 是否是护理险项目
        /// </summary>
        public bool IsNCIItem { get; set; }
        /// <summary>
        /// 医保耗材编码
        /// </summary>
        public string MCMaterialCode { get; set; }
        /// <summary>
        /// 定点服务机构耗材编码
        /// </summary>
        public string NSMaterialCode { get; set; }
        /// <summary>
        /// 耗材类型
        /// </summary>
        public string MaterialType { get; set; }
        /// <summary>
        /// 耗材名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 耗材等级
        /// </summary>
        public string MaterialLevel { get; set; }
        /// <summary>
        /// 国产进口标志
        /// </summary>
        public bool IsChinaProduct { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string Provider { get; set; }
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
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 用法
        /// </summary>
        public string Usage { get; set; }
        /// <summary>
        /// 注意事项
        /// </summary>
        public string Attention { get; set; }
        /// <summary>
        /// 指导价格
        /// </summary>
        public decimal? GuidePrice { get; set; }
        /// <summary>
        /// 最高单价
        /// </summary>
        public decimal? MaxPrice { get; set; }
        /// <summary>
        /// 护理险每月最大用量
        /// </summary>
        public int? NCIMonthlyMaxUsage { get; set; }
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
        public DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否需要更新
        /// </summary>
        public bool IsRequireUpdate { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新人Id
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDelete { get; set; }
    }
}

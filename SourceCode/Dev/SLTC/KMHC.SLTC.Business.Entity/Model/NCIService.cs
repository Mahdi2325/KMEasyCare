using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NCIService
    {
        /// <summary>
        /// 医保服务编码
        /// </summary>
        public string ServiceCode { get; set; }
        /// <summary>
        /// 医保标准分类码
        /// </summary>
        public string MCRuleId { get; set; }
        /// <summary>
        /// 服务项目名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 服务项目描述
        /// </summary>
        public string ServiceDesc { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Units { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 指导价格
        /// </summary>
        public decimal? GuidePrice { get; set; }
        /// <summary>
        /// 最高单价
        /// </summary>
        public decimal? MaxPrice { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 修订日期
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// 拼音助记码
        /// </summary>
        public string PinYin { get; set; }
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
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新人Id
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}

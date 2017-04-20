using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model.FinancialManagement
{
    public class RsChargeGroup
    {
        /// <summary>
        /// 套餐住民关系ID
        /// </summary>
        public int CGRId { get; set; }
        /// <summary>
        /// 套餐ID
        /// </summary>
        public string ChargeGroupId { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string ChargeGroupName { get; set; }
        
        /// <summary>
        /// 住民号
        /// </summary>
        public long FeeNo { get; set; }
        /// <summary>
        /// 项目启用否
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 套餐周期类型
        /// </summary>
        public string ChargeGroupPeriod { get; set; }
        /// <summary>
        /// 收费项目ID
        /// </summary>

        public int ChargeItemId { get; set; }
        /// <summary>
        /// 收费项目类型
        /// </summary>
        public int ChargeItemType { get; set; }
        /// <summary>
        /// 费用类别
        /// </summary>
        public string ChargeTypeId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int FeeItemCount { get; set; }
        /// <summary>
        /// 医保编号
        /// </summary>
        public string MCCode { get; set; }
        /// <summary>
        /// 院内编号
        /// </summary>
        public string NSCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否护理险
        /// </summary>
        public Nullable<bool> IsNciItem { get; set; }
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
        public decimal UnitPrice { get; set; }

        public int ConversionRatio { get; set; }

    }
}

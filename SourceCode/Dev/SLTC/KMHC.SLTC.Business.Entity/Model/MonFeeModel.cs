using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class MonFeeModel
    {
        /// <summary>
        /// 定点机构月费用ID
        /// </summary>
        public int NSMonFeeID { get; set; }
        /// <summary>
        /// 拨款ID
        /// </summary>
        public Nullable<int> NCIPayGrantID { get; set; }
        /// <summary>
        /// 定点服务机构ID
        /// </summary>
        public string NSID { get; set; }
        /// <summary>
        /// 定点服务机构编号
        /// </summary>
        public string NSNO { get; set; }
        /// <summary>
        /// 报销年月份
        /// </summary>
        public string YearMonth { get; set; }
        /// <summary>
        /// 总住民人数
        /// </summary>
        public int TotalResident { get; set; }
        /// <summary>
        /// 总住院天数
        /// </summary>
        public int TotalHospday { get; set; }
        /// <summary>
        /// 累计报销额度
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 累计报销额度
        /// </summary>
        public decimal TotalNCIPay { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> CreateTime { get; set; }
        /// <summary>
        /// 更新人编号
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public Nullable<System.DateTime> UpdateTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsCheck { get; set; }
    }
}

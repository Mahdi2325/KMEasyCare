using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class NCIDeductionModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 账单ID
        /// </summary>
        public string BillID { get; set; }
        /// <summary>
        /// 扣款类型 0:请假 1:经办机构操作
        /// </summary>
        public int DeductionType { get; set; }
        /// <summary>
        /// 扣款月份
        /// </summary>
        public string Debitmonth { get; set; }
        /// <summary>
        /// 请假记录ID
        /// </summary>
        public int? Leaveid { get; set; }
        /// <summary>
        /// 扣款天数
        /// </summary>
        public int? Debitdays { get; set; }
        /// <summary>
        /// 扣款金额
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 扣款原因
        /// </summary>
        public string DeductionReason { get; set; }
        /// <summary>
        /// 扣款状态0:未扣款 1:已扣款
        /// </summary>
        public int? DeductionStatus { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string Updateby { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public string Orgid { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDelete { get; set; }

        #region 拓展属性
        /// <summary>
        /// 住民编号
        /// </summary>
        public long?  FeeNo { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }

        #endregion
    }
}

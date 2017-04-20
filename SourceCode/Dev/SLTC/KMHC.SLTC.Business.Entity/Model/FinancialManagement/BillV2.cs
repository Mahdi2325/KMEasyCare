using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class BillV2
    {
        /// <summary>
        /// 账单ID
        /// </summary>
        public string BillId { get; set; }
        /// <summary>
        /// 缴费记录
        /// </summary>
        public Nullable<int> BillPayId { get; set; }
        /// <summary>
        /// 退款记录Id
        /// </summary>
        public Nullable<int> ReFundRecordId { get; set; }
        /// <summary>
        /// 住民No
        /// </summary>
        public long FeeNo { get; set; }
        /// <summary>
        /// 护理险项目总费用s
        /// </summary>
        public decimal NCIItemTotalCost { get; set; }

        /// <summary>
        /// 账单月
        /// </summary>
        public string BillMonth { get; set; }
        /// <summary>
        /// 自费项总费用
        /// </summary>
        public decimal SelfPay { get; set; }
        /// <summary>
        /// 报销标准
        /// </summary>
        public Nullable<decimal> NCIPayLevel { get; set; }
        /// <summary>
        /// 报销比例
        /// </summary>
        public Nullable<decimal> NCIPaysCale { get; set; }
        /// <summary>
        /// 护理险可报销费用
        /// </summary>
        public decimal NCIPay { get; set; }
        /// <summary>
        /// 个人自负
        /// </summary>
        public decimal NCIItemSelfPay { get; set; }
        /// <summary>
        /// 结算开始时间s
        /// </summary>
        public Nullable<System.DateTime> BalanceStartTime { get; set; }
        /// <summary>
        /// 结算结束时间
        /// </summary>
        public System.DateTime BalanceEndTime { get; set; }
        /// <summary>
        /// 住院天数
        /// </summary>
        public Nullable<int> HospDay { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 账单生成人
        /// </summary>
        public string BillCreator { get; set; }
        /// <summary>
        /// 结算经办人
        /// </summary>
        public string BalanceOperator { get; set; }
        /// <summary>
        /// 退款经办人
        /// </summary>
        public string RefundOperator { get; set; }
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
        /// <summary>
        /// 是否删除
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }

        public bool? IsFinancialClose { get; set; }

        #region add by BobWu
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmpName { get; set; }
        /// <summary>
        /// 账单生成人姓名
        /// </summary>
        public string BilleName { get; set; }
        /// <summary>
        /// 结算人姓名
        /// </summary>
        public string BalanceeName { get; set; }

        /// <summary>
        /// 退款人姓名
        /// </summary>
        public string RefundeName { get; set; }

        public decimal ThisSelfAmt { get; set; }

        public decimal LastPreAmt { get; set; }

        public string InvoiceNo { get; set; }

        public Nullable<decimal> TotalNciPay { get; set; }

        #endregion
        #region add by Amaya
        /// <summary>
        /// 机构ID
        /// </summary>
        public string OrgId { get; set; }
        #endregion
        #region add by Bob
        /// <summary>
        /// 入院日期
        /// </summary>
        public DateTime? InDate { get; set; }
        /// <summary>
        /// 出院日期
        /// </summary>
        public DateTime? OutDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InNo { get; set; }
        #endregion
    }

    public class BillV2List
    {
        public BillV2List()
        {
            BillV2Lists = new List<BillV2>();
        }
        public List<BillV2> BillV2Lists { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model.FinancialManagement
{
    public class SettleAccountModel
    {
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public Nullable<System.DateTime> BrithDate { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNo { get; set; }
        /// <summary>
        /// 医疗证号
        /// </summary>
        public string ResidengNo { get; set; }
        /// <summary>
        /// 户、人属性
        /// </summary>
        public string RSType { get; set; }
        /// <summary>
        /// 护理类别
        /// </summary>
        public string CareTypeId { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 住院号
        /// </summary>
        public long FeeNo { get; set; }
        /// <summary>
        /// 结算开始日
        /// </summary>
        public Nullable<System.DateTime> BalanceStarTtime { get; set; }
        /// <summary>
        /// 结算结束日
        /// </summary>
        public System.DateTime BalanceEndTime { get; set; }
        /// <summary>
        /// 住院天数
        /// </summary>
        public Nullable<int> HospDay { get; set; }
        /// <summary>
        /// 报销标准
        /// </summary>
        public decimal NCIPayLevel { get; set; }
        /// <summary>
        /// 报销比例
        /// </summary>
        public decimal NCIPayScale { get; set; }
        /// <summary>
        /// 报销金额
        /// </summary>
        public decimal NCIPay { get; set; }
        /// <summary>
        /// 报销金额
        /// </summary>
        public string InvoiceNo { get; set; }
        /// <summary>
        /// 发票号
        /// </summary>
        public string DiseaseDiag { get; set; }
        /// <summary>
        /// 疾病名称
        /// </summary>
        public decimal Amt1 { get; set; }
        /// <summary>
        /// 床位费
        /// </summary>
        public decimal Amt2 { get; set; }
        /// <summary>
        /// 护理费
        /// </summary>
        public decimal Amt3 { get; set; }
        /// <summary>
        /// 西药费
        /// </summary>
        public decimal Amt4 { get; set; }
        /// <summary>
        /// 中药费
        /// </summary>
        public decimal Amt5 { get; set; }
        /// <summary>
        /// 化验费
        /// </summary>
        public decimal Amt6 { get; set; }
        /// <summary>
        /// 诊疗费
        /// </summary>
        public decimal Amt7 { get; set; }
        /// <summary>
        /// 手术费
        /// </summary>
        public decimal Amt8 { get; set; }
        /// <summary>
        /// 检查费
        /// </summary>
        public decimal Amt9 { get; set; }
        /// <summary>
        /// 其他费用
        /// </summary>
        public decimal Amt10 { get; set; }
        /// <summary>
        /// 合计（元）
        /// </summary>
        public decimal Amt11 { get; set; }
        /// <summary>
        /// 甲类基本药物费
        /// </summary>
        public decimal Amt12 { get; set; }
        /// <summary>
        /// 乙类基本药物费
        /// </summary>
        public decimal Amt13 { get; set; }
        /// <summary>
        /// 自付金额(元)
        /// </summary>
        public decimal Amt14 { get; set; }
        /// <summary>
        /// 累计已报销金额(元)
        /// </summary>
        public decimal Amt16 { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class VisitPrescription
    {
        #region 基本属性
        public int PId { get; set; }
        public int? SeqNo { get; set; }
        public int? MedId { get; set; }
        //剂型
        public string Dosage { get; set; }
        //每次颗数
        public decimal? Qty { get; set; }
        //每剂剂量
        public string TakeQty { get; set; }
        //给药途径
        public string TakeWay { get; set; }
        //服药频率
        public string Freq { get; set; }
        //给药时间
        public string Freqtime { get; set; }
        //用法多少天
        public int? Freqday { get; set; }
        //用法多少次
        public int? Freqqty { get; set; }
        //长期药
        public bool? LongFlag { get; set; }
        //使用中
        public bool? UseFlag { get; set; }
        //处方开始日期
        public Nullable<System.DateTime> StartDate { get; set; }
        //处方结束日期
        public Nullable<System.DateTime> EndDate { get; set; }

        //定点服务机构ID
        public string NsId { get; set; }
        //住民ID
        public long? FeeNo { get; set; }
        //总量
        public decimal TotalQty { get; set; }
        //单价
        public decimal UnitPrice { get; set; }
        //总价
        public decimal Cost { get; set; }
        //是否是护理险项目
        public int IsNciItem { get; set; }
        //是否来自套餐
        public int IsChargeGroupItem { get; set; }

        //药品通用中文名称
        public string CnName { get; set; }
        //
        public string Units { get; set; }
        //时间
        public DateTime TakeTime { get; set; }
        //经办人
        public string Operator { get; set; }
        //
        public int? DrugId { get; set; }
        public string Description { get; set; }
        public string OrgId { get; set; }
        #endregion

        #region 附加属性
        //药品名称
        public string EngName { get; set; }
        public string ChnName { get; set; }
        public string FreqName { get; set; }

        //每颗剂量单位
        public string MedKind { get; set; }

        // 住院序号      
        //public long? FeeNo { get; set; }
        //医师姓名
        public string VisitDoctor { get; set; }
        //医师姓名
        public string VisitDoctorName { get; set; }
        //医院名称
        public string VisitHospName { get; set; }
        //科别
        public string VisitDeptName { get; set; }
        //就医类型
        public string VisitType { get; set; }

        public int? TakeDays { get; set; }
        #endregion

        public virtual VisitDocRecords VisitDocRecords { get; set; }
    }
}


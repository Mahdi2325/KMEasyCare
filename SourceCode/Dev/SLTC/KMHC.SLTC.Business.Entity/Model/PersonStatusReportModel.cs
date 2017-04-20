using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class PersonStatusReportModel
    {
        public PersonStatusReportModel()
        {
            #region 资格证待遇申请统计												
            StatisticalNum CertlastYearNum = new StatisticalNum();
            StatisticalNum ByCertlastYearNum = new StatisticalNum();
            StatisticalNum UnAppCertlastYearNum = new StatisticalNum();
            StatisticalNum CancelCertlastYearNum = new StatisticalNum();
            StatisticalNum AppCertYearNum = new StatisticalNum();
            StatisticalNum ByCertYearNum = new StatisticalNum();
            StatisticalNum UnAppCertYearNum = new StatisticalNum();
            StatisticalNum CancelCertYearNum = new StatisticalNum();
            StatisticalNum AppCertSearchYearNum = new StatisticalNum();
            StatisticalNum ByCertSearchYearNum = new StatisticalNum();
            StatisticalNum UnAppCertSearchYearNum = new StatisticalNum();
            StatisticalNum CancelCertSearchYearNum = new StatisticalNum();
            #endregion

            #region 长照信息统计
            StatisticalNum DrugEntryNum = new StatisticalNum();
            StatisticalNum NSCPLNum = new StatisticalNum();
            StatisticalNum BillV2Num = new StatisticalNum();
            StatisticalSum CostNum = new StatisticalSum();
            #endregion

            #region 在院/不在院情况统计				
            StatisticalNum InHospNum = new StatisticalNum();
            StatisticalNum OutHospOfOtherNum = new StatisticalNum();
            StatisticalNum OutHospOfDeadNum = new StatisticalNum();
            StatisticalNum LeaveHospNum = new StatisticalNum();
            #endregion
        }

        #region 资格证待遇申请统计		
        #region 资格证待遇申请统计 截止2016年年12月31日			
        /// <summary>
        /// 截止2016年年12月31日	 申请数量
        /// </summary>
        public StatisticalNum AppCertlastYearNum { get; set; }

        /// <summary>
        /// 截止2016年年12月31日	 通过数量
        /// </summary>
        public StatisticalNum ByCertlastYearNum { get; set; }

        /// <summary>
        /// 截止2016年年12月31日	 未通过数量
        /// </summary>
        public StatisticalNum UnAppCertlastYearNum { get; set; }

        /// <summary>
        /// 截止2016年年12月31日	 取消数量
        /// </summary>
        public StatisticalNum CancelCertlastYearNum { get; set; }
        #endregion

        #region 资格证待遇申请统计 截止2017年3月31日		
        /// <summary>
        /// 截止2017年3月31日		 申请数量
        /// </summary>
        public StatisticalNum AppCertYearNum { get; set; }

        /// <summary>
        /// 截止2017年3月31日		 通过数量
        /// </summary>
        public StatisticalNum ByCertYearNum { get; set; }

        /// <summary>
        /// 截止2017年3月31日		 未通过数量
        /// </summary>
        public StatisticalNum UnAppCertYearNum { get; set; }

        /// <summary>
        /// 截止2017年3月31日		 取消数量
        /// </summary>
        public StatisticalNum CancelCertYearNum { get; set; }
        #endregion

        #region 资格证待遇申请统计 查询期间			
        /// <summary>
        /// 查询期间			 申请数量
        /// </summary>
        public StatisticalNum AppCertSearchYearNum { get; set; }

        /// <summary>
        /// 查询期间		 通过数量
        /// </summary>
        public StatisticalNum ByCertSearchYearNum { get; set; }

        /// <summary>
        /// 查询期间		 未通过数量
        /// </summary>
        public StatisticalNum UnAppCertSearchYearNum { get; set; }

        /// <summary>
        /// 查询期间		 取消数量
        /// </summary>
        public StatisticalNum CancelCertSearchYearNum { get; set; }
        #endregion
        #endregion

        #region 长照信息查询
        /// <summary>
        /// 药品录入数
        /// </summary>
        public StatisticalNum DrugEntryNum { get; set; }
        /// <summary>
        /// 新增护理计划数
        /// </summary>
        public StatisticalNum NSCPLNum { get; set; }
        /// <summary>
        /// 生成账单数
        /// </summary>
        public StatisticalNum BillV2Num { get; set; }
        /// <summary>
        /// 生成账单数
        /// </summary>
        public StatisticalSum CostNum { get; set; }
        #endregion

        #region 在院/不在院情况统计	
        /// <summary>
        /// 在院人数
        /// </summary>
        public StatisticalNum InHospNum { get; set; }

        /// <summary>
        /// 出院人数(其他)
        /// </summary>
        public StatisticalNum OutHospOfOtherNum { get; set; }

        /// <summary>
        /// 出院人数(死亡)
        /// </summary>
        public StatisticalNum OutHospOfDeadNum { get; set; }

        /// <summary>
        /// 请假人数
        /// </summary>
        public StatisticalNum LeaveHospNum { get; set; }
        #endregion
    }

    public class StatisticalNum
    {
        /// <summary>
        /// 总计
        /// </summary>
        public int? TotalNum { get; set; }

        /// <summary>
        /// 县医院人数
        /// </summary>
        public int? XyyNum { get; set; }

        /// <summary>
        /// 祈康医院人数
        /// </summary>
        public int? QkyyNum { get; set; }

        /// <summary>
        /// 健民医院人数
        /// </summary>
        public int? JmyyNum { get; set; }
    }
    public class StatisticalSum
    {
        /// <summary>
        /// 总计
        /// </summary>
        public double? TotalNum { get; set; }

        /// <summary>
        /// 县医院人数
        /// </summary>
        public double? XyyNum { get; set; }

        /// <summary>
        /// 祈康医院人数
        /// </summary>
        public double? QkyyNum { get; set; }

        /// <summary>
        /// 健民医院人数
        /// </summary>
        public double? JmyyNum { get; set; }
    }
}

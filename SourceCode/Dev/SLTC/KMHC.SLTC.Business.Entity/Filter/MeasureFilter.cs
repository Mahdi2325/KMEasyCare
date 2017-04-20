using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class Measure
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public long? FeeNo { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public string OrgID { get; set; }
        /// <summary>
        /// 量测编码
        /// </summary>
        public string MeasureItemCode { get; set; }
        /// <summary>
        /// 量测人
        /// </summary>
        public string MeaSuredPerson { get; set; }
        /// <summary>
        /// 量测人
        /// </summary>
        public string MeaSuredPersonName { get; set; }
        /// <summary>
        /// 量测时间
        /// </summary>
        public DateTime? MeaSuredTime { get; set; }
        public DateTime? MeaSureTime { get; set; }
    }

    public class MeasureFilter : Measure
    {
        /// <summary>
        /// 量测值
        /// </summary>
        public float MeaSuredValue { get; set; }
    }

    public class WeightFilter : Measure
    {
        /// <summary>
        /// 身高
        /// </summary>
        public decimal? Height { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// BMI
        /// </summary>
        public decimal? BMI { get; set; }
        /// <summary>
        /// BMI结果
        /// </summary>
        public string BMI_RS { get; set; }
    }

    //用在WebApi ExtNursingDataController
    public class MeasureDataFilter
    {
        public long? FeeNo { get; set; }
        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
    }
}






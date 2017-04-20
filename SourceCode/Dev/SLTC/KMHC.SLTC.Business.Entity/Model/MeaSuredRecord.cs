using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class CommonMeasureRecord
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public Nullable<long> FeeNo { get; set; }
        /// <summary>
        /// 量测时间
        /// </summary>
        public DateTime? MeaSureTime { get; set; }
        /// <summary>
        /// 量测编码
        /// </summary>
        public string MeasureItemCode { get; set; }        
        /// <summary>
        /// 量测人
        /// </summary>
        public string MeaSuredPerson { get; set; }
        /// <summary>
        /// 量测人姓名
        /// </summary>
        public string MeaSuredPersonName { get; set; }
    }
    public class MeaSuredRecord : CommonMeasureRecord
    {
        /// <summary>
        /// 量测值
        /// </summary>
        public float MeaSuredValue { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Createby { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string Updateby { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string  Source { get; set; }
        /// <summary>
        /// 机构id
        /// </summary>
        public string Orgid { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool?  IsDelete { get; set; }

    }

    public class WeightRecord : CommonMeasureRecord
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


    public class MeasureData
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string MeasureRs { get; set; }
    }

    public class MeaSuredRecordModel : MeasureData
    {

        public float? MeasureValue { get; set; }
        public DateTime? MeasureTime { get; set; }     
    }

    public class WeightModel : MeasureData
    {
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? BMI { get; set; }
    }

    public class MeasureTotalHistory
    {
        public long feeno { get; set; }
        public DateTime Date { get; set; }
        public string MeasureRs { get; set; }
    }
}

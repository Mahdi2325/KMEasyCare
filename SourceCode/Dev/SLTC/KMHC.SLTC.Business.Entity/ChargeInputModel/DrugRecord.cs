using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.ChargeInputModel
{
    public class DrugRecord
    {
        public int DrugrecordId { get; set; }
        //药品ID
        public int DrugId { get; set; }
        //定点服务机构ID
        public string NsId { get; set; }
        //住民ID
        public long FeeNo { get; set; }
        //药品通用中文名称
        public string CnName { get; set; }
        //剂型
        public string Form { get; set; }
        //转换比
        public int ConversionRatio { get; set; }
        //单位
        public string Units { get; set; }
        //开药单位
        public string PrescribeUnits { get; set; }
        //开药数量
        public decimal DrugQty { get; set; }
        //计价数量
        public decimal Qty { get; set; }
        //单价
        public decimal Unitprice { get; set; }
        //总价
        public decimal Cost { get; set; }
        //剂量
        public decimal Dosage { get; set; }
        //途径
        public string Takeway { get; set; }
        //频率
        public string Ferq { get; set; }
        //时间
        public DateTime TakeTime { get; set; }
        //经办人
        public string Operator { get; set; }
        //备注
        public string Comment { get; set; }
        //是否是护理险项目
        public bool IsNciItem { get; set; }
        //是否来自套餐
        public bool IsChargeGroupItem { get; set; }
        //
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? IsDelete { get; set; }      
    }

    public class DrugRecordFilter
    {
        //耗材ID 
        public int DrugrecordId { get; set; }
        //住民ID
        public long FeeNo { get; set; }
    }

    public class DrugRec
    {
        public IList<DrugRecord> Data { get; set; }

    }
}

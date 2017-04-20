using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.ChargeInputModel
{
    public class MaterialRecord
    {
        //耗材记录ID 
        public int MaterialRecordId { get; set; }
        //住民ID
        public long FeeNo { get; set; }
        //耗材ID 
        public int? MaterialId { get; set; }
        //定点服务机构ID 
        public string NsId { get; set; }
        //耗材名称 
        public string MaterialName { get; set; }
        //单位 
        public string Units { get; set; }
        //总量
        public decimal Qty { get; set; }
        //单价
        public decimal Unitprice { get; set; }
        //总价
        public decimal Cost { get; set; } 
        //途径 
        public string Takeway { get; set; }
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

        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? IsDelete { get; set; }
    }

    public class MaterialRecordFilter
    {
        //耗材ID 
        public int MaterialRecordId { get; set; }
        //住民ID
        public long FeeNo { get; set; }
    }

    public class MaterialRec
    {
        public IList<MaterialRecord> Data { get; set; }

    }

}

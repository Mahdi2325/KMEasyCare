using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.ChargeInputModel
{
    public class CommonRecord
    {
        //项目ID
        public int Id { get; set; }
        //定点服务机构ID
        public string NsId { get; set; }
        //入住号
        public long FeeNo { get; set; }
        //名称
        public string Name { get; set; }
        //剂型
        public string Form { get; set; }
        //单位
        public string Units { get; set; }
        //规格
        public string Spec { get; set; }
        //总量
        public decimal? Qty { get; set; }
        //单价
        public decimal Unitprice { get; set; }
        //剂量
        public decimal Dosage { get; set; }
        //剂量
        public string Freq { get; set; }
        //总价
        public decimal Cost { get; set; }
        //途径 
        public string Takeway { get; set; }
        //时间 
        public DateTime? TakeTime { get; set; }
        //状态
        public Nullable<int> Status { get; set; }
        //经办人
        public string Operator { get; set; }
        //备注 
        public string Comment { get; set; }
        //是否是护理险项目 
        public bool IsNciItem { get; set; }
        //是否来自套餐 
        public bool IsChargeGroupItem { get; set; }
        //M:耗材 S:服务
        public string RecordType { get; set; }

        public bool? IsDelete { get; set; }

        public DateTime? CreateTime { get; set; }
    }

    public class CommonRecordList
    {
        public List<CommonRecord> Data { get; set; }
    }

    public class CommonRec
    {
        public string RecType { get; set; }

        public DrugRec drugRec { get; set; }
        public MaterialRec materialRec { get; set; }
        public ServiceRec serviceRec { get; set; }
    }

    public class DeleteRec
    {
        public string RecType { get; set; }
        public  int RecId { get; set; }
        public int CgcrId { get; set; }
    }
}

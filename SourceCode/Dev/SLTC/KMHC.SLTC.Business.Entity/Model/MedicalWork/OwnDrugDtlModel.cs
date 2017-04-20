using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model.MedicalWork
{
    public class OwnDrugDtlModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 自带药品序号
        /// </summary>
        public int OwnDrugId { get; set; }
        /// <summary>
        /// 药品ID
        /// </summary>
        public int DrugId { get; set; }
        /// <summary>
        /// 医保编码
        /// </summary>
        public string MCDrugCode { get; set; }
        /// <summary>
        /// 院内码
        /// </summary>
        public string NSDrugCode { get; set; }
        /// <summary>
        /// 中文名
        /// </summary>
        public string CNName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Nullable<decimal> Qty { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Units { get; set; }
        /// <summary>
        /// 生成厂商
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
        
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }


    public class OwnDrugDtlList
    {
        public OwnDrugDtlList()
        {
            OwnDrugDtlLists = new List<OwnDrugDtlModel>();
        }

        /// <summary>
        /// 修改的编号
        /// </summary>
        public int UpdateOwnDrugId { get; set; }
        public List<OwnDrugDtlModel> OwnDrugDtlLists { get; set; }
    }
}

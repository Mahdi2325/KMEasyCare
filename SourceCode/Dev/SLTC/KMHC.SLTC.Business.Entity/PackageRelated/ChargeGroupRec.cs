using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.PackageRelated
{
    public class ChargeGroupRec
    {
        #region 基本属性
        //关系ID
        public int CgcrId { get; set; }
        //套餐ID
        public string ChargeGroupId { get; set; }
        //住民FeeNo
        public long FeeNo { get; set; }
        //费用项目类型
        //public int? ChargeRecordType { get; set; }
        //收费项目使用记录ID
        //public int? ChargeRecordId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool? IsDelete { get; set; }
        #endregion

        #region 扩展属性
        //套餐Name
        public string ChargeGroupName { get; set; }
        #endregion
    }

    public class ChargeGroupRecFilter
    {
        //住民FeeNo
        public long FeeNo { get; set; }

        public int ChargeRecordId { get; set; }

        public int ChargeRecordType { get; set; }

        public string ChargeGroupId { get; set; }

        public int CgcrId { get; set; }

    }

    public class ChargeItemData
    {
        public ChargeGroupRec ChargeGroupRec { get; set; }
        public List<CHARGEITEM> ChargeItem { get; set; }
    }
}

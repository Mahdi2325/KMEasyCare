using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class FeeRecordModel
    {
        /// <summary>
        /// 费用记录ID
        /// </summary>
        public string FeeRecordID { get; set; }
        /// <summary>
        /// 账单ID
        /// </summary>
        public string BillId { get; set; }
        /// <summary>
        /// 收费项目使用记录类型
        /// </summary>
        public int ChargeRecordType { get; set; }
        /// <summary>
        /// 收费项目使用记录ID
        /// </summary>
        public int ChargeRecordID { get; set; }
        /// <summary>
        /// 住民ID
        /// </summary>
        public long FeeNo { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Count { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// 是否是护理险项目
        /// </summary>
        public bool IsNCIItem { get; set; }
        /// <summary>
        /// 是否是退款记录
        /// </summary>
        public Nullable<bool> IsRefundRecord { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Createby { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> CreateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public Nullable<System.DateTime> UpdateTime { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }
    }

    public class FeeRecordBaseInfo : FeeRecordModel
    {
        /// <summary>
        /// 机构ID
        /// </summary>
        public string NSID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// 项目的社保编号
        /// </summary>
        public string MCCode { get; set; } 
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime TakeTime { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Spec { get; set; }
        /// <summary>
        /// 剂型
        /// </summary>
        public string Form { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Units { get; set; }
        /// <summary>
        /// 剂量
        /// </summary>
        public decimal Dosage { get; set; }

        /// <summary>
        /// 途径
        /// </summary>
        public string Takeway { get; set; }

        /// <summary>
        /// 频率
        /// </summary>
        public string Ferq { get; set; }

        /// <summary>
        /// 经办者
        /// </summary>
        public string Operator{ get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 是否来自套餐
        /// </summary>
        public bool IsChargeGroupItem { get; set; }
        /// <summary>
        /// 费用类别Id
        /// </summary>
        public string ChargeTypeId { get; set; }
        /// <summary>
        /// 医保类别Id
        /// </summary>
        public string MCType { get; set; }

    }

    public class BillV2FeeRecord
    {
        public BillV2FeeRecord()
        {
            drugRecordList = new List<FeeRecordBaseInfo>();
            materialRecordList = new List<FeeRecordBaseInfo>();
            serviceRecordList = new List<FeeRecordBaseInfo>();
            residentBalance = new RegNCIInfo();
        }

        public RegNCIInfo residentBalance { get; set; }
        public List<FeeRecordBaseInfo> drugRecordList { get; set; }
        public List<FeeRecordBaseInfo> materialRecordList { get; set; }
        public List<FeeRecordBaseInfo> serviceRecordList { get; set; }
    }


    public class BillV2FeeList
    {
        public BillV2FeeList()
        {
            feeRecordList = new List<FeeRecordBaseInfo>();
            regInformation = new List<RegInfo>();
        }
        public List<RegInfo> regInformation { get; set; }
        public List<FeeRecordBaseInfo> feeRecordList { get; set; }
    }
    

    public class RegInfo
    {
        /// <summary>
        /// RegNO
        /// </summary>
        public int RegNO { get; set; }

        /// <summary>
        /// FeeNO
        /// </summary>
        public long FeeNO { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 费用核算时间开始时间
        /// </summary>
        public DateTime STime { get; set; }
        /// <summary>
        /// 费用核算时间结束时间
        /// </summary>
        public DateTime ETime { get; set; }
        /// <summary>
        /// 住院天数
        /// </summary>
        public double InHosDays { get; set; }
        /// <summary>
        /// 住民编号
        /// </summary>
        public string ResidentNo { get; set; }
        /// <summary>
        /// 住院次数
        /// </summary>
        public int InHosCount { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public string Floor { get; set; } 
        /// <summary>
        /// 房间
        /// </summary>
        public string RoomNo { get; set; }
        /// <summary>
        /// 床号
        /// </summary>
        public string BedNo { get; set; }

        				
    }

    public class BillV2Info
    {

        public BillV2Info()
        {
            feeRecordList = new List<FeeRecordBaseInfo>();
            NCIDudeList = new List<NCIDeductionModel>();
        }
        /// <summary>
        /// 住民编号
        /// </summary>
        public long  Feeno { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime STime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime ETime { get; set; }

        public string BillMonth { get; set; }
        public string IdNo { get; set; }

        public List<FeeRecordBaseInfo> feeRecordList { get; set; }

        public List<NCIDeductionModel> NCIDudeList { get; set; }

    }


    //public enum FeeRecordEnum
    //{
    //    drugrecord = 1,
    //    materialrecord = 2,
    //    servicerecord = 3,
    //}

    public  class  ChargeRecord
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否删除数据
        /// </summary>
        public bool? IsDelete { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int? ChargeRecordType { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public int? ChargeRecordID { get; set; }
    }
}

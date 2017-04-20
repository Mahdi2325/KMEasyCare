using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace KMHC.SLTC.Business.Entity
{
    public enum EnumResponseStatus
    {
        Unauthorized = -2,
        ExceptionHappened = -1,
        Success = 0
    }

    public enum EnumCodeKey
    {
        Demo,
        LCRegNo,
        DCRegNo,
        OrgId,
        GroupId,
        BillNo,
        PayBillNo,
        ChargeGroupNo,
        RoleId,
        StaffId,
        LeaveHospId,
        ManufactureNo,
        GoodNo,
        GoodsLoanNo,
        GoodsSaleNo,
        EmpSysPre,//累加前缀,用於生产每个机构的固定前缀
        EmpPrefix,//员工编号前缀
        QuestionId,//Add by Duke
        LimitedId,//Add by Duke
        RoomId,//Add by Duke
        FloorId,//Add by Duke
        MeaSuredRecordId, //add by Amaya
        NCIEvaluate,
    }

    /// <summary>
    /// 固定资费周期
    /// </summary>
    public enum EnumPeriod
    {
        Day,
        Month
    }

    public enum EnumBillState
    {
        [DescriptionAttribute("Open")]
        /// <summary>
        /// 新产生账单
        /// </summary>
        Open,
        [DescriptionAttribute("Close")]
        /// <summary>
        /// 关账
        /// </summary>
        Close,
        [DescriptionAttribute("Cancel")]
        /// <summary>
        /// 取消
        /// </summary>
        Cancel
    }

    /// <summary>
    /// 角色类型 
    /// SuperAdmin 是预制的 
    /// Admin类型的Role在机构维护界面生成，一个机构只有一个Admin类型Role，调用SuperAdmin类型Role作为模板，生成相应的数据，保存时取消的构选的模块要同步该机构的所有Role
    /// Normal类型的Role在角色维护界面生成，一个机构可以有多个Normal类型Role
    /// </summary>
    public enum EnumRoleType
    {
        SuperAdmin,
        Admin,
        Normal
    }

    public enum MajorType
    {
        社工照顾计划 = 1,
        护理照顾计划 = 2
    }

    /// <summary>
    /// 费用记录状态
    /// </summary>
    public enum RecordStatus
    {
        [DescriptionAttribute("创建费用记录")]
        /// <summary>
        /// 创建费用记录
        /// </summary>
        Create = 0,
        [DescriptionAttribute("生成账单")]
        /// <summary>
        /// 生成账单
        /// </summary>
        GenerateBill = 1,
        [DescriptionAttribute("缴费")]
        /// <summary>
        /// 缴费
        /// </summary>
        Charge = 2,
        [DescriptionAttribute("退费")]
        /// <summary>
        /// 退费
        /// </summary>
        Refund = 8
    }

    /// <summary>
    /// 账单状态
    /// </summary>
    public enum BillStatus
    {
        [DescriptionAttribute("未缴费")]
        /// <summary>
        /// 未缴费
        /// </summary>
        NoCharge = 0,
        [DescriptionAttribute("已缴费")]
        /// <summary>
        /// 已缴费
        /// </summary>
        Charge = 2,
        [DescriptionAttribute("已退费")]
        /// <summary>
        /// 已退费
        /// </summary>
        Refund = 8,
        [DescriptionAttribute("已上传")]
        /// <summary>
        /// 已上传
        /// </summary>
        Uploaded = 20
    }

    /// <summary>
    /// 收费项目类型
    /// </summary>
    public enum ChargeItemType
    {
        [DescriptionAttribute("药品")]
        /// <summary>
        /// 药品
        /// </summary>
        Drug = 1,
        [DescriptionAttribute("耗材")]
        /// <summary>
        /// 耗材
        /// </summary>
        Material = 2,
        [DescriptionAttribute("服务")]
        /// <summary>
        /// 服务
        /// </summary>
        Service = 3,
    }
    public enum BedStatus
    {
        [DescriptionAttribute("已使用")]
        Used,
        [DescriptionAttribute("空置")]
        Empty,
        [DescriptionAttribute("已预定")]
        Subscribe,
        [DescriptionAttribute("停用")]
        Disable
    }

    public enum QuestionCode
    {
        ADL = 4,
        MMSE = 5,
        IADL = 7,
        SORE = 10,
        FALL = 11,
    }
    /// <summary>
    /// 咨询登记订金状态
    /// </summary>
    public enum EarnestStatus
    {
        [DescriptionAttribute("未交订金")]
        UnPain = 1,
        [DescriptionAttribute("已交订金")]
        Paid = 2,
        [DescriptionAttribute("已退订金")]
        Returned = 3
    }

    /// <summary>
    /// 扣款类型
    /// </summary>
    public enum DeductionType
    {
        [DescriptionAttribute("请假")]
        LeaveHosp = 0,
        [DescriptionAttribute("经办机构操作")]
        NCIOpr = 1,
        [DescriptionAttribute("结案操作")]
        IpdRegOut = 2
    }

    /// <summary>
    /// 扣款状态
    /// </summary>
    public enum DeductionStatus
    {
        [DescriptionAttribute("未扣款")]
        UnCharge = 0,
        [DescriptionAttribute("已扣款")]
        Charged = 1
    }

    public enum NCIPStatusEnum
    {
        /// <summary>
        /// 已创建
        /// </summary>
        Created = 0,
        /// <summary>
        /// 已撤回
        /// </summary>
        Withdrawn = 1,
        /// <summary>
        /// 待审核
        /// </summary>
        Pending = 10,
        /// <summary>
        /// 审核通过
        /// </summary>
        Passed = 20,
        /// <summary>
        /// 已拨款
        /// </summary>
        Appropriated = 30,
        /// <summary>
        /// 审核不通过
        /// </summary>
        NotPassed = 90,
    }

    public enum ReportFileType
    {
        Doc = 0,
        Xls = 1,
        Pdf = 2
    }
    public enum OrgValue
    {
        [DescriptionAttribute("32975608-1")]
        /// <summary>
        /// xyy
        /// </summary>
        Xyy,
        [DescriptionAttribute("35904143-1")]
        /// <summary>
        /// Qkyy
        /// </summary>
        Qkyy,
        [DescriptionAttribute("67466499-7")]
        /// <summary>
        /// Jmyy
        /// </summary>
        Jmyy
    }
}

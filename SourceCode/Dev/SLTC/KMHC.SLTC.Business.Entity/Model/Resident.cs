using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Resident
    {
        public string Name { get; set; }
        //昵称
        public string Nickname { get; set; }
        public string Sex { get; set; }
        //服务类型
        public string ServiceType { get; set; }

        //住民类型
        public string RsType { get; set; }

        //住民身份
        public string RsStatus { get; set; }
        //个人照片
        public string ImgUrl { get; set; }
        //学员号
        public string ResidengNo { get; set; }

        public string ResidentInfo { get; set; }
        //年龄
        public int Age { get; set; }
        //床号
        public string Bunk { get; set; }
        //用户ID
        public string UserID { get; set; }
        //出生日期
        public DateTime? BirthDay { get; set; }
        //身份证号
        public string IdNo { get; set; }
        public long FeeNo { get; set; }
        public string FeeNoString { get; set; }
        public string PType { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }
        public string DeptNo { get; set; }
        public string NurseNo { get; set; }
        public string Carer { get; set; }
        public string BedNo { get; set; }
        public string RoomNo { get; set; }
        public string RoomName { get; set; }
        public string BedClass { get; set; }
        public string BedType { get; set; }
        public string Floor { get; set; }
        public string FloorName { get; set; }
        public string BedKind { get; set; }
        public string SickFlag { get; set; }
        public string RoomFlag { get; set; }
        public string ProtFlaf { get; set; }
        public string IpdFlag { get; set; }
        public string IpdFlagName { get; set; }
        public Nullable<System.DateTime> OutDate { get; set; }
        public string DangerFlag { get; set; }
        public Nullable<decimal> DepositAmt { get; set; }
        public Nullable<decimal> PrepayAmt { get; set; }
        public string CtrlFlag { get; set; }
        public string CtrlReason { get; set; }
        public string NursingTips { get; set; }
        public string CarerTips { get; set; }
        public string StateFlag { get; set; }
        public string StateReason { get; set; }
        public string OrgId { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string Nutritionist { get; set; }
        public string Physiotherapist { get; set; }
        public string Doctor { get; set; }

        public bool? IsFinancialClose { get; set; }

        public DateTime? FinancialCloseTime { get; set; }

        #region 附加属性
        public string BedStatus { get; set; }
        public string NurseName { get; set; }
        public string CarerName { get; set; }
        public string NutritionistName { get; set; }
        public string PhysiotherapistName { get; set; }
        public string DoctorName { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string DiseaseDiag { get; set; }
        public bool IsHasNCI { get; set; }
        public string OrgName { get; set; }
        public string CareType { get; set; }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class RSDList
    {
        public string Name { get; set; }
        //昵称
        public string Nickname { get; set; }
        public string Sex { get; set; }
        //服务类型
        public string ServiceType { get; set; }
        //护理等级
        public string NurLevel { get; set; }
        //住民入住态度
        public string RsAtt { get; set; }
        //家属对托养人的入住态度
        public string FmyRsAtt { get; set; }
        //护理等级评估小组意见
        public string NurAssSugg { get; set; }
        //中心意见或建议
        public string OrgSugg { get; set; }
        //家属对住民自由出入意见
        public string ImpComment { get; set; }
        //个人照片
        public string ImgUrl { get; set; }
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
        /// <summary>
        /// 住民类型
        /// </summary>
        public string RsType { get; set; }
        /// <summary>
        /// 住民身份
        /// </summary>
        public string RsStatus { get; set; }
        public string ResidengNo { get; set; }
        public long? FeeNo { get; set; }
        public string FeeNoString { get; set; }
        public string PType { get; set; }
        public DateTime? InDate { get; set; }
        public string DeptNo { get; set; }
        public string NurseNo { get; set; }
        public string NurseName { get; set; }
        public string Carer { get; set; }
		public string BedType { get; set; }

		public string SexType { get; set; }
		public string Prestatus { get; set; }
		public string InsbedFlag { get; set; }

        /// <summary>
        /// 主责护士
        /// </summary>
        public string CarerName { get; set; }
        public string BedNo { get; set; }
        public string OldBedNo { get; set; }
        public string RoomNo { get; set; }
        public string RoomName { get; set; }//房间名称20161201
        public string Area { get; set; }
        public string RoomType { get; set; }

        public string OldRoomNo { get; set; }
        public string BedClass { get; set; }
        public string Floor { get; set; }
        public string FloorName { get; set; }//楼层名称20161201
        public string OldFloor { get; set; }
        public string BedKind { get; set; }
        public bool? SickFlag { get; set; }
        public bool? RoomFlag { get; set; }
        public bool? ProtFlaf { get; set; }
        public string IpdFlag { get; set; }
        public string IpdFlagName { get; set; }
        public DateTime? OutDate { get; set; }
        public bool? DangerFlag { get; set; }
        public decimal? DepositAmt { get; set; }
        public decimal? PrepayAmt { get; set; }
        public bool? CtrlFlag { get; set; }
        public string CtrlReason { get; set; }
        public string NursingTips { get; set; }
        public string CarerTips { get; set; }
        public string StateFlag { get; set; }
        public string StateReason { get; set; }
        public string OrgId { get; set; }
        public int? RegNo { get; set; }
        public string Nutritionist { get; set; }
        public string NutritionistName { get; set; }
        public string Physiotherapist { get; set; }
        public string PhysiotherapistName { get; set; }
        public string Doctor { get; set; }
        public string DoctorName { get; set; }

        public string City2 { get; set; }

        public string Address2 { get; set; }

        public string Address2Dtl { get; set; }

        public string ContactPhone { get; set; }

        public string CertNo { get; set; }

        public Nullable<int> CertStatus { get; set; }
    }
}


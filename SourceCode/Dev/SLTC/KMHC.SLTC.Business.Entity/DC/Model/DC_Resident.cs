using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
   public class DC_Resident
    {
    
           public string Name { get; set; }
           //昵称
           public string NickName { get; set; }
           public string Sex { get; set; }
           //服务类型
           public string ServiceType { get; set; }
           //个人照片
           public string ImgUrl { get; set; }
           //学员号
           public string ResidentNo { get; set; }
           public string RegNo { get; set; }
           //年龄
           public int Age { get; set; }
           //区域
           public string StationCode { get; set; }
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
           public string SickFlag { get; set; }
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

           public string OrgName { get; set; }
           public string Nutritionist { get; set; }
           public string Physiotherapist { get; set; }
           public string Doctor { get; set; }
           public DateTime? BirthDate { get; set; }

           public string BirthPlace { get; set; }

       
    }
}


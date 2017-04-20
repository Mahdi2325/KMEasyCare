using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_ReferrallistsModel
    {
        public long Id { get; set; }
        public long? FeeNo { get; set; }
        public string SiologicalState { get; set; }
        public string ProblemStatement { get; set; }
        public string ReferralPurpose { get; set; }
        public string DocumentInfo { get; set; }
        public DateTime? ReferralDate { get; set; }
        public string SocialWorker { get; set; }
        public string Supervisor { get; set; }
        public string ReferralUnit { get; set; }
        public string ReferralResult { get; set; }
        public DateTime? ReplyDate { get; set; }
        public string UnitContactor { get; set; }
        public string UnitPhone { get; set; }
        public string UnitFax { get; set; }
        public string UnitName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy{ get; set; }
        public string OrgId { get; set; }

        public string RegName { get; set; }
        public string Sex { get; set; }
        public DateTime? InDate { get; set; }
        public string SuretyName { get; set; }
        public string SuretyPhone { get; set; }
        public string OrgName { get; set; }
        public int Age { get; set; }
        public string OriginPlace { get; set; }//籍贯
        public string Phone { get; set; }//个案电话
        public string PermanentAddress { get; set; }//户籍地址
        public string LivingAddress { get; set; }//居住地址
        public string Language { get; set; }//沟通语言
        public string MerryState { get; set; }//婚姻状况
        public string ObstacleManual { get; set; }//手册别
        public string DiseaseInfo { get; set; }//疾病状况
        //public string SiologicalState { get; set; }//生理状况
        public string IdNo { get; set; }
        public string No { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Religion { get; set; }//宗教信仰

    }
}


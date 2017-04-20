using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class DC_RegFileModel
    {
        public string RegNo { get; set; }
        public string RegName { get; set; }
        public string NickName { get; set; }
        public string Sex { get; set; }
        public string BirthPlace { get; set; }
        public string OriginPlace { get; set; }
        public DateTime? BirthDate { get; set; }
        public string IdNo { get; set; }
        public string PType { get; set; }
        public string Phone { get; set; }
        public string LivingAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Language { get; set; }
        public string Education { get; set; }
        public string Profession { get; set; }
        public string MerryState { get; set; }
        public string Religion { get; set; }
        public string EconomicSources { get; set; }
        public string LivCondition { get; set; }
        public string SourceType { get; set; }
        public string ObstacleManual { get; set; }
        public string SuretyName { get; set; }
        public int? SuretyAge { get; set; }
        public string SuretyUnit { get; set; }
        public string SuretyTitle { get; set; }
        public string SuretyAddress { get; set; }
        public string SuretyEmail { get; set; }
        public string SP { get; set; }
        public string SuretyPhone { get; set; }
        public string SuretyMobile { get; set; }
        public string ContactName1 { get; set; }
        public int? ContactAge1 { get; set; }
        public string ContactUnit1 { get; set; }
        public string ContactTitle1 { get; set; }
        public string ContactAddress1 { get; set; }
        public string ContactEmail1 { get; set; }
        public string ContactPhone1 { get; set; }
        public string ContactMobile1 { get; set; }
        public string CP1 { get; set; }
        public string ContactName2 { get; set; }
        public int? ContactAge2 { get; set; }
        public string ContactUnit2 { get; set; }
        public string ContactTitle2 { get; set; }
        public string ContactAddress2 { get; set; }
        public string ContactEmail2 { get; set; }
        public string ContactPhone2 { get; set; }
        public string CP2 { get; set; }
        public string ContactMobile2 { get; set; }
        public string DiseaseInfo { get; set; }
        public string EcologicalMap { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string CheckDate { get; set; }
        public string CheckedBy { get; set; }
        public string OrgId { get; set; }
        //住民日字号，表 <IPDREG>
        public string ResidentNo { get; set; }
        public DateTime? InDate { get; set; }
        public string NurseAidesName { get; set; }//照服员姓名
        public string OrgName { get; set; }
        public int Age { get; set; }
		public string IpdFlag { get; set; }
		public long FeeNo { get; set; }
		public string StationCode { get; set; }
    }
}


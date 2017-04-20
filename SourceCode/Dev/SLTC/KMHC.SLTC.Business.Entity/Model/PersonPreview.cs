using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class PersonPreview
    {
        public int RegNo { get; set; }
        public long FeeNo { get; set; }
        public string Name { get; set; }
        public string ResidengNo { get; set; }
        public string Sex { get; set; }
        public Nullable<System.DateTime> Brithdate { get; set; }
        public string IdNo { get; set; }
        public string Education { get; set; }
        public string Race { get; set; }
        public string Political { get; set; }
        public string Skill { get; set; }
        public string Experience { get; set; }
        public string Habit { get; set; }
        public string ReligionCode { get; set; }
        public string Language { get; set; }
        public string MerryFlag { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public List<RelationDtl> RelationDtl { get; set; }
        public string ImgUrl { get; set; }
        public DateTime? InDate { get; set; }
        public string StateFlag { get; set; }

        public string servicetype { get; set; }
        public string CAddress1 { get; set; }
        public string CAddress2 { get; set; }
        public string ContactPhone { get; set; }
        public string City2 { get; set; }
        public string Address2 { get; set; }
        public string Address2dtl { get; set; }
        public string City1 { get; set; }
        public string Address1 { get; set; }
        public string Address1dtl { get; set; }
        public string Email { get; set; }
        public string StateReason { get; set; }

        public string For_SOURCE { get; set; }
        public string Payment { get; set; }
        public string Living { get; set; }

        public string AIDTOOLS { get; set; }
        public string Provedoc { get; set; }
        public string Insurance { get; set; }
        public string Special { get; set; }
        public string Status { get; set; }
        public string Barrier_1 { get; set; }
        public string DIS_Level { get; set; }
        public string Barrier_2 { get; set; }
        public string Getdisable { get; set; }
        public string bigbigsick { get; set; }
        public string Day_1 { get; set; }
        public string Economic { get; set; }
        public string Enter_reason { get; set; }

        public string Add_UNIT { get; set; }
        public string Enter_type { get; set; }
        public string Emer_save { get; set; }
        public string Case_instrest { get; set; }
        public string Need_hot { get; set; }
        public string diet { get; set; }
        public string Diagnosis { get; set; }
        public string Health { get; set; }
        public string Infectious_disease { get; set; }
        public string Over_sensitive { get; set; }
        public string Body_condition { get; set; }
        public string Take_care_need { get; set; }
        public string Family_disease_history { get; set; }
        public string Tackle { get; set; }
        public string Demand { get; set; }
        public string Genogram { get; set; }


        public string SOURCETYPE { get; set; }
        public string INSURANCEDESC { get; set; }
        public string PROCDOC { get; set; }
        public string INSMARK { get; set; }
        public string CASETYPE { get; set; }
        public string DISABDEGREE { get; set; }
        public DateTime? BOOKISSUEDATE { get; set; }
        public DateTime? BOOKEXPDATE { get; set; }
        public bool? ISREEVAL { get; set; }
        public DateTime? DISABILITYEVALDATE { get; set; }
        public string ECONOMYFLAG { get; set; }
        public string ICDDIAGNOSI { get; set; }
        public string EffectiveTime { get; set; }

        public string DISEASEDIAG { get; set; }
        public string BOOKTYPE { get; set; }
        public string EATHABITS { get; set; }
        public string requirements { get; set; }
        public string ISIdentificationText { get; set; }

        public string FloorName { get; set; }
        public string RoomName { get; set; }
        public string SubsidyUnit { get; set; }

        public bool? DISABILITYCATE1 { get; set; }
        public bool? DISABILITYCATE2 { get; set; }
        public bool? DISABILITYCATE3 { get; set; }
        public bool? DISABILITYCATE4 { get; set; }
        public bool? DISABILITYCATE5 { get; set; }
        public bool? DISABILITYCATE6 { get; set; }
        public bool? DISABILITYCATE7 { get; set; }
        public bool? DISABILITYCATE8 { get; set; }
       
    }
}

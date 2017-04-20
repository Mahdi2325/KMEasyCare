using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Person
    {

        //public Person()
        //{
        //    RegDisData = new RegDisData();
        //}
        public int RegNo { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string ResidengNo { get; set; }
        public string Sex { get; set; }
        public Nullable<System.DateTime> Brithdate { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }
        public Nullable<int> Age { get; set; }
        public string IdNo { get; set; }
        public string Education { get; set; }

        public string Race { get; set; }
        public string Political { get; set; }
        public string Skill { get; set; }
        public string WorkCode { get; set; }
        public string Experience { get; set; }
        public string Title { get; set; }
        public string Habit { get; set; }
        public string ReligionCode { get; set; }
        public string Language { get; set; }
        public string CommunicateType { get; set; }
        public string BrithPlace { get; set; }
        public string RsType { get; set; }
        public string RsStatus { get; set; }
        public string MerryFlag { get; set; }
        public Nullable<int> Child_Boy { get; set; }
        public Nullable<int> Child_Girl { get; set; }
        public string Caregiver { get; set; }
        public string LiveCondition { get; set; }
        public string Constellations { get; set; }
        public string BloodType { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string PersonalHistory { get; set; }
        public string FamilyHistory { get; set; }
        public string FloorName { get; set; }
        public string RoomName { get; set; }
        public string SsNo { get; set; }
        public string InfecFlag { get; set; }
        public string RHType { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string OrgId { get; set; }
        public string DiseaseDiag { get; set; }
        public long? FeeNo { get; set; }
        public Relation Relation { get; set; }
        public List<RelationDtl> RelationDtls { get; set; }
        public List<AttachFile> AttachArchives { get; set; }
        public string Floor { get; set; }
        public string BedNo { get; set; }
        public string IpdFlag { get; set; }
        public RegDisData RegDisData { get; set; }
        public int Status { get; set; }
    }

    public class RegDisData
    {
        public int RegNo { get; set; }
        public Regdiseasehis Regdiseasehis { get; set; }
        public IList<RegdiseasehisDtl> RegdiseasehisDtl { get; set; }
    }
}
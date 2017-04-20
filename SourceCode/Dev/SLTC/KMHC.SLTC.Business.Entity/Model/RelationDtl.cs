using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class RelationDtl
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public string Contrel { get; set; }
        public string Kinship { get; set; }
        public string RelationType { get; set; }
        public bool LivingFlag { get; set; }
        public string Education { get; set; }
        public string MerryFlag { get; set; }
        public string HealthFlag { get; set; }
        public string DeathFlag { get; set; }
        public string WorkCode { get; set; }
        public bool EconomyFlag { get; set; }
        public string IdNo { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Skype { get; set; }
        public string MSN { get; set; }
        public string Email { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public string EconomyAbility { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}






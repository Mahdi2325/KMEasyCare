using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Health
    {
        //public string Allergy { get; set; }
        //public string HealthInfo { get; set; }
        //public string Eathabits { get; set; }
        //public Nullable<decimal> Cal { get; set; }
        //public string CookFlag { get; set; }
        //public string NotcookReason { get; set; }
        //public Nullable<System.DateTime> StartDate { get; set; }
        //public Nullable<System.DateTime> EndDate { get; set; }
        //public string CatheterFlag { get; set; }
        //public string DemandFlag { get; set; }
        //public string FamilyHistory { get; set; }
        //public long FeeNo { get; set; }
        //public string OrgId { get; set; }
        //public Nullable<int> RegNo { get; set; }
        public long FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public string ALLERGY { get; set; }
        public string HEALTHINFO { get; set; }
        public string EATHABITS { get; set; }
        public Nullable<decimal> CAL { get; set; }
        public Nullable<bool> COOKFLAG { get; set; }
        public string NOTCOOKREASON { get; set; }
        public Nullable<System.DateTime> STARTDATE { get; set; }
        public Nullable<System.DateTime> ENDDATE { get; set; }
        public Nullable<bool> CATHETERFLAG { get; set; }
        public Nullable<bool> TRACHEOSTOMYFLAG { get; set; }
        public Nullable<bool> NASOGASTRICFLAG { get; set; }
        public string DEMANDDESC { get; set; }
        public string ORGID { get; set; }
        public string CATHETERSIZE { get; set; }
        public string TRACHEOSTOMYSIZE { get; set; }
        public string NASOGASTRICSIZE { get; set; }
        public Nullable<bool> STOMAFLAG { get; set; }
        public string STOMASIZE { get; set; }
    }
}






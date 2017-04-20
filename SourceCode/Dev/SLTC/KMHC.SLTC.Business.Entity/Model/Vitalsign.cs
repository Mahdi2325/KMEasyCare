using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Vitalsign
    {
        public long SeqNo { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> SBP { get; set; }
        public Nullable<int> DBP { get; set; }
        public Nullable<int> Pulse { get; set; }
        public Nullable<decimal> Bodytemp { get; set; }
        public Nullable<int> Breathe { get; set; }
        public Nullable<int> Oxygen { get; set; }
        public Nullable<decimal> BloodSugar { get; set; }
        public string BSType { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string Coma { get; set; }
        public Nullable<int> Pain { get; set; }
        public Nullable<int> Bowels { get; set; }
        public string ClassType { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public string OrgId { get; set; }
        public Nullable<int> Bloodsugar { get; set; }

        public decimal? InValue { get; set; }
        public decimal? OutValue { get; set; }
        public DateTime Date { get; set; }

        public string ItemCode { get; set; }
        public float? Measuredvalue { get; set; }

    }
}






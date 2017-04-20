using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class UnPlanWeightInd
    {
        public long Id { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public Nullable<decimal> ThisWeight { get; set; }
        public Nullable<decimal> ThisHeight { get; set; }
        public Nullable<decimal> Waist { get; set; }
        public Nullable<decimal> Hipline { get; set; }
        public Nullable<System.DateTime> ThisRecDate { get; set; }
        public Nullable<decimal> LastWeight { get; set; }
        public Nullable<decimal> DiffValue { get; set; }
        public Nullable<decimal> DiffValueRate { get; set; }
        public Nullable<System.DateTime> LastrecDate { get; set; }
        public Nullable<decimal> BMI { get; set; }
        public string BMIResults { get; set; }
        public Nullable<decimal> B3Weight { get; set; }
        public Nullable<decimal> B3DiffValue { get; set; }
        public Nullable<decimal> B3DiffValueRate { get; set; }
        public Nullable<decimal> B6Weight { get; set; }
        public Nullable<decimal> B6DiffValue { get; set; }
        public Nullable<decimal> B6DiffValueRate { get; set; }
        public Nullable<bool> UnPlanFlag { get; set; }
        public string RecordBy { get; set; }
        public string OrgId { get; set; }
        public string RecordNameBy { get; set; }

        public string Name { get; set; }
        public string FloorName { get; set; }
        public string RoomName { get; set; }

        public Nullable<decimal> KneeLen { get; set; }
        
    }
}






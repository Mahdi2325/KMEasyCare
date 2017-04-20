using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class InfectionInd
    {
        public long SeqNo { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string IfcType { get; set; }
        public Nullable<System.DateTime> IfcDate { get; set; }
        public string RecordBy { get; set; }
        public string RecordNameBy { get; set; }
        public Nullable<System.DateTime> RecDate { get; set; }
        public string Category { get; set; }
        public Nullable<bool> H72Flag { get; set; }
        public Nullable<bool> IfcFlag { get; set; }
        public Nullable<bool> CatheterFlag { get; set; }
        public Nullable<decimal> ItemScore { get; set; }
        public string SecType { get; set; }
        public Nullable<System.DateTime> SecStartDate { get; set; }
        public Nullable<System.DateTime> SecEndDate { get; set; }
        public Nullable<int> SecDays { get; set; }
        public string ClinicalSymptom { get; set; }
        public string DoctorDiag { get; set; }
        public Nullable<bool> B3AntiFlag { get; set; }
        public Nullable<bool> AntitreatFlag { get; set; }
        public string AntitreatType { get; set; }
        public string Improvement { get; set; }
        public string Description { get; set; }
        public string OrgId { get; set; }
        public string ItemNo { get; set; }
        //public List<InfectionSympotm> Detail { get; set; }
    }
}






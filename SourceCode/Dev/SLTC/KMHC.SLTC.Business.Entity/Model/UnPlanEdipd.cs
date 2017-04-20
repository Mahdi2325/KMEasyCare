using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class UnPlanEdipd
    {
        public long Id { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string RecordBy { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }
        public Nullable<bool> ChargeFlag { get; set; }
        public string IpdDiag { get; set; }
        public Nullable<bool> EpdFlag { get; set; }
        public string IpdCause { get; set; }
        public Nullable<bool> H72Ipd { get; set; }
        public Nullable<bool> UnPlanFlag { get; set; }
        public Nullable<bool> CatheterFlag { get; set; }
        public string HospName { get; set; }
        public string EscortPeople { get; set; }
        public string EscortRelation { get; set; }
        public string IpdDesc { get; set; }
        public string MrSummary { get; set; }
        public Nullable<decimal> DepositAmt { get; set; }
        public string DepositDesc { get; set; }
        public string OutReason { get; set; }
        public Nullable<bool> OutFlag { get; set; }
        public Nullable<System.DateTime> OutDate { get; set; }
        public Nullable<int> IpdDays { get; set; }
        public string PayoutWay { get; set; }
        public Nullable<decimal> PayAmt { get; set; }
        public string BedType { get; set; }
        public string Description { get; set; }
        public string OrgId { get; set; }
    }
}






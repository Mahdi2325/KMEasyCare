using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class VisitorInOut
    {
        public long Id { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public string Interviewee { get; set; }
        public string RecordBy { get; set; }
        public Nullable<System.DateTime> RecordTime { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string VisitType { get; set; }
        public string VisitorName { get; set; }
        public string VisitorSex { get; set; }
        public string VisitorIdNo { get; set; }
        public string VisitorCompany { get; set; }
        public string VisitorTel { get; set; }
        public Nullable<bool> IsRegVisit { get; set; }
        public string Appellation { get; set; }
        public string BloodRelationShip { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string OrgId { get; set; }

        public string IpdFlag { get; set; } 

    }

}
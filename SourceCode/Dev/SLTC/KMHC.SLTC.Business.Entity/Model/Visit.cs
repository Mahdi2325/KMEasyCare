using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Visit
    {
        public long Id { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string VisitType { get; set; }
        public string Name { get; set; }
        public string Contrel { get; set; }
        public string Kinship { get; set; }
        public Nullable<decimal> Temperature { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
        public Nullable<int> RegNo { get; set; }
    }
}






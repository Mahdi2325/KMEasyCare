using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class LeaveHosp
    {
        public int Id { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public Nullable<long> FeeNo { get; set; }

        public string ResidengNo { get; set; }
        public string Name { get; set; }
        public string BedNo { get; set; }

        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> LeHour { get; set; }
        public string LeNote { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string Address { get; set; }
        public string ContName { get; set; }
        public string ContRel { get; set; }
        public string ContTel { get; set; }
        public string LeType { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string OrgId { get; set; }

        public string IpdFlag { get; set; }

        public Nullable<bool> IsDelete { get; set; }
        public int  Status { get; set; }
    }
}






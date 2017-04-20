using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NursingHandover
    {
        public int Id { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string Content_D { get; set; }
        public string Content_E { get; set; }
        public string Content_N { get; set; }
        public Nullable<System.DateTime> Recdate_D { get; set; }
        public Nullable<System.DateTime> Recdate_E { get; set; }
        public Nullable<System.DateTime> Recdate_N { get; set; }
        public string Recordby_D { get; set; }
        public string Recordby_E { get; set; }
        public string Recordby_N { get; set; }
        public string Nurse_D { get; set; }
        public string Nurse_E { get; set; }
        public string Nurse_N { get; set; }
        public string PrintFlag { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
    }
}






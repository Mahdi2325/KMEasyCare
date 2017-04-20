using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Employee
    {
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string EmpGroup { get; set; }
        public string EmpGroupName { get; set; }
        public string IdNo { get; set; }
        public Nullable<System.DateTime> Brithdate { get; set; }
        public string BrithPlace { get; set; }
        public string Sex { get; set; }
        public string BloodType { get; set; }
        public string RthType { get; set; }
        public string MerryFlag { get; set; }
        public string HomeTelNo { get; set; }
        public string Zip1 { get; set; }
        public string Address1 { get; set; }
        public string Zip2 { get; set; }
        public string Address2 { get; set; }
        public string ContName { get; set; }
        public string ContRelation { get; set; }
        public string ContTelphone { get; set; }
        public string ContAddress { get; set; }
        public string JobTitle { get; set; }
        public string Education { get; set; }
        public bool? Status { get; set; }
        public string DeptNo { get; set; }
        public string HiredType { get; set; }
        public string JobType { get; set; }
        public Nullable<bool> NativesFlag { get; set; }
        public string LigiousFaith { get; set; }
        public Nullable<bool> DisabilityFlag { get; set; }
        public string Nationality { get; set; }
        public string OrgId { get; set; }
    }
}






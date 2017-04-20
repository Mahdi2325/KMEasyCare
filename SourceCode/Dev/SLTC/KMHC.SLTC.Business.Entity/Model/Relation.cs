using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Relation
    {
        public long FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string Zip1 { get; set; }
        public string City1 { get; set; }
        public string Address1 { get; set; }
        public string Address1dtl { get; set; }
        public string Zip2 { get; set; }
        public string City2 { get; set; }
        public string Address2 { get; set; }
        public string Address2dtl { get; set; }
        public bool HouseholdFlag { get; set; }
        public string ContactPhone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PhotoPath { get; set; }
        public string FamilyPath { get; set; }
        public string PaymentPerson { get; set; }
        public string Kinship { get; set; }
        public string BillAddress { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
    }
}






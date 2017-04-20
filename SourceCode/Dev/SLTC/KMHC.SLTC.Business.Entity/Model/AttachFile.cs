using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class AttachFile
    {
        public long Id { get; set; }
        public long FeeNo { get; set; }
        public string DocType { get; set; }
        public string DocPath { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string OrgId { get; set; }
    }
}






using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Notice
    {
        public int Id { get; set; }
        public string Subjects { get; set; }
        public string Contents { get; set; }
        public string Doctype { get; set; }
        public string DocumentNo { get; set; }
        public string RecordBy { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public Nullable<System.DateTime> ExprieDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
    }
}

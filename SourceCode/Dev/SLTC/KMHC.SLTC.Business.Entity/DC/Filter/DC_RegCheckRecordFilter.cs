using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_RegCheckRecordFilter
    {
        public string OrgId { get; set; }
        public long? FeeNo { get; set; }
        public string RegNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool DisplayType { get; set; }
        public bool? TraceStatus { get; set; }
        public string IdNo { get; set; }
        public string RegName { get; set; }
        public string CheckTemplateCode { get; set; }
    }

    public class DC_RegCheckRecordDataFilter
    {
        public long? RecordId { get; set; }
    }
    
    public class DC_RegNoteRecordFilter
    {
        public string OrgId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class DC_NoteFilter
    {
        public string OrgId { get; set; }
        public string NoteName { get; set; }
        public sbyte? IsShow { get; set; }
    }

    public class DC_RegVisitRecordFilter
    {
        public string OrgId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class CheckTemplateFilter
    {
        public string OrgId { get; set; }
    }
    
}

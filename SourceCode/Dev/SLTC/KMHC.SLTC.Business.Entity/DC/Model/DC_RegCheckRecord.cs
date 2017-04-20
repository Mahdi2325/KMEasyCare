using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
    public class DC_RegCheckRecordModel
    {
        public long RecordId { get; set; }
        public string OrgId { get; set; }
        public string RegNo { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public string CheckTemplateCode { get; set; }
        public string ActionUserCode { get; set; }
        public Nullable<bool> IsAbnormal { get; set; }
        public Nullable<bool> TraceStatus { get; set; }
        public Nullable<System.DateTime> TraceDate { get; set; }
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IdNo { get; set; }
        public string RegName { get; set; }
        public string NickName { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthdate { get; set; }
        public int ChildRecourdCount { get; set; }
    }

    public class DC_RegCheckRecordDtlModel
    {
        public long RecordId { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public string CheckTemplateCode { get; set; }
        public string CheckTemplateName { get; set; }
        public string CheckResult { get; set; }
        public Nullable<bool> IsAbnormal { get; set; }
        public Nullable<bool> TraceStatus { get; set; }
    }

    public class DC_RegCheckRecordDataModel
    {
        public long DataId { get; set; }
        public Nullable<long> RecordId { get; set; }
        public string CheckItemCode { get; set; }
        public string CheckItemValue { get; set; }
        public string LowBound { get; set; }
        public string UpBound { get; set; }
        public string Severityname { get; set; }
    }

    public class CheckTemplateModel
    {
        public string CheckTemplateCode { get; set; }
        public string CheckTemplateName { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string OrgId { get; set; }
    }


    public class DC_RegNoteRecordModel
    {
        public long RecordId { get; set; }
        public string ActionUserCode { get; set; }
        public string ActionUserName { get; set; }
        public DateTime? NoteDate { get; set; }
        public string RegNo { get; set; }
        public int? NoteId { get; set; }
        public string NoteName { get; set; }
        public string NoteContent { get; set; }
        public sbyte? ViewStatus { get; set; }
        public DateTime? ViewDate { get; set; }
        public string OrgId { get; set; }
    }

    public class DC_NoteModel
    {
        public int NoteId { get; set; }
        public string ActionUserCode { get; set; }
        public string ActionUserName { get; set; }
        public string NoteName { get; set; }
        public string NoteContent { get; set; }
        public int? ShowNumber { get; set; }
        public sbyte? IsShow { get; set; }
        public string OrgId { get; set; }
    }

    public class DC_RegVisitRecordModel
    {
        public long RecordId { get; set; }
        public string ActionUserCode { get; set; }
        public string ActionUserName { get; set; }
        public DateTime? VisitDate { get; set; }
        public string RegNo { get; set; }
        public string VisitName { get; set; }
        public string VisitContent { get; set; }
        public string OrgId { get; set; }
    }

}

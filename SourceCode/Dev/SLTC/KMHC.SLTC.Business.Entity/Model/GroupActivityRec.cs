namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class GroupActivityRec
    {
        public int Id { get; set; }
        public string ActivityName { get; set; }
        public string ActivityType { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public Nullable<System.DateTime> EventDate { get; set; }
        public Nullable<int> EventHours { get; set; }
        public string EventPlace { get; set; }
        public string LeaderName { get; set; }
        public string LeaderName1 { get; set; }
        public string LeaderName2 { get; set; }
        public string ActivityResults { get; set; }
        public Nullable<int> EvalResults { get; set; }
        public Nullable<int> AttendNumber { get; set; }
        public string GroupCompetence { get; set; }
        public string IndividualSummary { get; set; }
        public string Suggestion { get; set; }
        public string Description { get; set; }
        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string AttendNo { get; set; }
        public string AttendName { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
    }
}

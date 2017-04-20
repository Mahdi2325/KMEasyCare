using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class CloseCase
    {
        public long FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string PalliativeCare { get; set; }
        public string PalliativeCareFile { get; set; }
        public bool CloseFlag { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public string CloseReason { get; set; }
        public bool TrainingData { get; set; }
        public bool WillsFlag { get; set; }
        public string WillsDesc { get; set; }
        public string BodyProcess { get; set; }
        public string BodyProcessDesc { get; set; }
        public string BodyKeepPlace { get; set; }
        public string EstateExecutor { get; set; }
        public string EstateProcess { get; set; }
        public string FuneralProcess { get; set; }
        public string BodyProcess_Exe { get; set; }
        public string BodyProcessdesc_Exe { get; set; }
        public string BodyKeepPlace_Exe { get; set; }
        public string EstateExecutor_Exe { get; set; }
        public string EstateProcess_Exe { get; set; }
        public bool DeathFlag { get; set; }
        public Nullable<System.DateTime> DeathDate { get; set; }
        public string DeathReason { get; set; }
        public string DeathPlace { get; set; }
        public Nullable<System.DateTime> MeetingDate { get; set; }
        public string Participants { get; set; }
        public string MeetingNotes { get; set; }
        public string OrgId { get; set; }
    }
}






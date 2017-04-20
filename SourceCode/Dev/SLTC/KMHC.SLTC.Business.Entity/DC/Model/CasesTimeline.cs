using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
    public class CasesTimeline
    {
        public long FEENO { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string NURSEAIDES { get; set; }
        public string ECOLOGICALMAP { get; set; } //use for DC_MultiteaMcarePlanEvalModel 
        public List<TimelineContainer> TimelineContainer { get; set; }
    }

    public class TimelineContainer
    {
        public int Tag { get; set; }
        public string MajorType { get; set; }
        public List<Timeline> Timeline { get; set; }
    }

    public class Timeline
    {
        public string MMSESCORE { get; set; }
        public string IADLSCORE { get; set; }
        public string ADLSCORE { get; set; }
        public string GODSSCORE { get; set; }
        public string StartDate { get; set; }
        public string Title { get; set; }
        public string Title1 { get; set; }
        public string Header1 { get; set; }
        public string Header2 { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public List<TimelineText> Plan { get; set; }
    }

    public class TimelineText
    {
        public string MAJORTYPE { get; set; }
        public string QUESTIONTYPE { get; set; }
        public string CPDIA { get; set; } //项目
        public string QUESTIONDESC { get; set; }//问题描述
        public List<string> ACTIVITY { get; set; }
        public string TRACEDESC { get; set; }
    }

    public class timelineContainer
    {
        public timelineContext timeline { get; set; }
    }

    public class timelineContext
    {
        public string headline { get; set; }
        public string type { get; set; }
        public string startDate { get; set; }
        public string text { get; set; }
        public asset asset { get; set; }
        public List<timelineData> date{get;set;}

    }


    public class timelineData
    {
        public string headline { get; set; }
        public string startDate { get; set; }
        public string text { get; set; }
        public asset asset { get; set; }
    }

    public class asset
    {
        public string media { get; set; }
        public string credit { get; set; }
        public string caption { get; set; }
    }
}


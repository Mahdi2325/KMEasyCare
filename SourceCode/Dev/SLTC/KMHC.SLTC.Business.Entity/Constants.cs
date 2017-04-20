using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity
{
    public class Constants
    {
        public const string DiseaseTypeTreeKey = "DiseaseTypeTree";
        public const string AreaKey = "Area";
        public const string CodeValuesKey = "CodeValuesKey";
        public const string TimelineText = "<i><span class='c1'>{0}时间轴</span><span class='c2'></span></i>";
        public const string TimelineTextHeaderS = "<div class='timeline-group-div'><label class='timeline-task-text'>{0}：</label> ";
        public const string TimelineTextHeaderC = "<label style='float:right;color:#5e89af'>IADL:{0}分;MMSE:{1}分;ADL:{2}分;GDS:{3}分;</label>";
        public const string TimelineTextHeaderE = "</div><div class='hack timeline-hack-border'></div> ";
        public const string TimelineContentHeaderS = "<div style='width:100%' class='timeline-group-div'><div class='timeline-group-div timeline-group-header' > ";
        public const string TimelineContentHeaderC = "</div><div class='timeline-group-content'> ";
        public const string TimelineContentHeaderE = "	</div> </div><div class='hack timeline-hack-border'></div>";
        public const string TimelineContentLable = "<div class='timeline-group-content-lable'><div><div class='timeline-group-div'>{0}</div></div></div>";
        public const string SecretKey = "kjewlytroioi3459werj*()_fgj";
        /// <summary>
        /// 住民类型
        /// </summary>
        public static Dictionary<string, string> ResidentType = new Dictionary<string, string>(){
             {"001","社会"}, {"002","三无"},{"003","残疾"}
        };

        /// <summary>
        /// MacAddress and HardDiskId
        /// </summary>
        public static Dictionary<string, string> ServerIdentify = new Dictionary<string, string>(){
             {"XENSRC PVDISK SCSI Disk Device","2E:F6:03:3B:3E:A4"}
        };

    }
}
